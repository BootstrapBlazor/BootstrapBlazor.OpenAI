// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.Extensions.Configuration;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace BootstrapBlazor.Components;

/// <summary>
/// OpenAI API 使用 GPT-3 预训练生成式转换器
/// </summary>
public class OpenAiBBService
{
    [NotNull]
    public OpenAIService? OpenAiService;

    //
    // 摘要:
    //     The OpenAI API uses API keys for authentication. Visit your API Keys page to
    //     retrieve the API key you'll use in your requests. Remember that your API key
    //     is a secret! Do not share it with others or expose it in any client-side code(browsers,
    //     apps). Production requests must be routed through your own backend server where
    //     your API key can be securely loaded from an environment variable or key management
    //     service.
    public string? OpenAIKey { get; set; }

    public OpenAiBBService(IConfiguration? Config)
    {
        OpenAIKey = OpenAIKey ?? Config!["OpenAIKey"];
        if (OpenAIKey != null)
            Init();
    }

    public virtual void Init(string? openAIKey = null)
    {
        OpenAIKey = openAIKey ?? OpenAIKey;
        if (OpenAIKey != null)
        {
            OpenAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = OpenAIKey
            });

            OpenAiService.SetDefaultModelId(Models.Davinci);
        }
    }

    /// <summary>
    /// ChatGPT 聊天
    /// </summary>
    /// <remarks>ChatGPT，全称聊天生成预训练转换器（英語：Chat Generative Pre-trained Transformer），是OpenAI开发的人工智能聊天机器人程序，于2022年11月推出。 该程序使用基于GPT-3.5架构的大型语言模型並以强化学习训练</remarks>
    /// <returns></returns>
    public async Task<string?> ChatGPT(string prompt = "曾几何时")
    {
        //ChatGPT Sample

        /***
        GPT - 3.5 - turbo 模型是以一系列消息作为输入，并将模型生成的消息作为输出。
        每个消息都是一个对象，包含一个字符串和一个布尔值，该布尔值指示消息是用户消息还是系统消息。
        系统消息是由模型生成的消息，而用户消息是用户输入的消息。
        模型将用户消息与系统消息一起传递给模型，以便模型可以生成下一个系统消息。
        模型将生成的消息作为系统消息返回，以便您可以将其显示给用户。
        您可以将多个消息传递给模型，以便模型可以生成多个系统消息。

        系统 FromSystem 消息有助于设置助手的行为。在上面的例子中，助手被指示 “你是一个得力的助手”。
        用户 FromAssistant 消息有助于指导助手。 就是用户说的话，向助手提的问题。
        助手 FromUser 消息有助于存储先前的回复。这是为了持续对话，提供会话的上下文。

        语言模型以称为 tokens 的块读取文本。在英语中，一个 token 可以短至一个字符或长至一个单词（例如，a 或 apple），在某些语言中，token 可以比一个字符更短，也可以比一个单词长。
        例如，字符串 “ChatGPT is great！” 被编码成六个 token：[“Chat”, “G”, “PT”, “ is”, “ great”, “!”]。
        1.API 调用中的 token 总数会影响：
        2.API 调用成本：因为您需要为为每个 token 支付费用
        3.API 调用响应时间：因为写入更多令牌需要更多时间
        4.API 调用是否有效：因为令牌总数必须是 低于模型的最大限制（gpt - 3.5 - turbo - 0301 为 4096 个令牌）

        另请注意，非常长的对话更有可能收到不完整的回复。例如，一个长度为 4090 个 token 的 gpt-3.5-turbo 对话将在只回复了 6 个 token 后被截断。

        ***/

        var completionResult = await OpenAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                //ChatMessage.FromSystem("你是一个翻译家."),
                //ChatMessage.FromUser("将我发你的英文句子翻译成中文，你不需要理解内容的含义作出回答。"),
                //ChatMessage.FromUser("Draft an email or other piece of writing."),
                //res 起草一封电子邮件或其他写作材料
                //ChatMessage.FromAssistant("洛杉矶道奇队赢得了 2020 年世界大赛冠军."),
                ChatMessage.FromUser(prompt)
            },
            Model = Models.ChatGpt3_5Turbo,
            MaxTokens = 300,//完成时生成的最大令牌数
            Temperature = null, //浮点数，控制模型的输出的多样性。值越高，输出越多样化。值越低，输出越简单。默认值为 0.5。
            N = 1, //整数，生成的候选项的数量。默认值为 1。
        });
        if (completionResult.Successful)
        {
            System.Console.WriteLine(completionResult.Choices.First().Message.Content);
            return completionResult.Choices.First().Message.Content;
        }
        return null;
    }

    /// <summary>
    /// Completions 完成
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<string?> Completions(string prompt = "曾几何时")
    {
        //Completions Sample

        /***
         给定一个提示，该模型将返回一个或多个预测的 Completions，并且还可以返回每个位置的替代标记的概率。
         */

        var completionResult = await OpenAiService.Completions.CreateCompletion(new CompletionCreateRequest()
        {
            Prompt = prompt,
            Model = Models.TextDavinciV3,
            MaxTokens = 300, //完成时生成的最大令牌数。
            Temperature = null, //浮点数，控制模型的输出的多样性。值越高，输出越多样化。值越低，输出越简单。默认值为 0.5。
            N=1, //整数，生成的候选项的数量。默认值为 1。
        });

        if (completionResult.Successful)
        {
            System.Console.WriteLine(completionResult.Choices.FirstOrDefault());
            return completionResult.Choices.FirstOrDefault()?.Text;
        }
        else
        {
            if (completionResult.Error == null)
            {
                throw new Exception("Unknown Error");
            }
            System.Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            return $"{completionResult.Error.Code}: {completionResult.Error.Message}";
        }
    }

    /// <summary>
    /// Completions Stream 流式完成, 连续令牌接收结果
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<string?> CompletionsStream(string prompt = "曾几何时")
    {
        //Completions Stream Sample

        /***
         * 如果您想更快地获得响应，您可以在生成时“流式传输”完成。 这允许您在整个完成完成之前开始打印或以其他方式处理完成的开始。
         */

        var completionResult = OpenAiService.Completions.CreateCompletionAsStream(new CompletionCreateRequest()
        {
            Prompt = prompt,
            MaxTokens = 300
        }, Models.Davinci);

        await foreach (var completion in completionResult)
        {
            if (completion.Successful)
            {
                System.Console.Write(completion.Choices.FirstOrDefault()?.Text);
                return completion.Choices.FirstOrDefault()?.Text;
            }
            else
            {
                if (completion.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                System.Console.WriteLine($"{completion.Error.Code}: {completion.Error.Message}");
                return $"{completion.Error.Code}: {completion.Error.Message}";
            }
        }
        System.Console.WriteLine("Complete");
        return null;
    }

    /// <summary>
    /// DALL-E是一个可以通过文本描述中生成图像的人工智能程序。于2021年1月5日由OpenAI发表。 DALL-E通过120亿参数版本的GPT-3 Transformer模型来理解自然语言输入并生成相应的图片。它既可以生成现实的对象，也能够生成现实中不存在的对象。
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>

    public async Task<string?> DALLE_CreateImage(string prompt = "镭射猫眼", bool base64 = true)
    {
        //DALL·E Sample
        var imageResult = await OpenAiService.Image.CreateImage(new ImageCreateRequest
        {
            Prompt = prompt,
            N = 1,
            Size = StaticValues.ImageStatics.Size.Size512,
            ResponseFormat = base64 ? StaticValues.ImageStatics.ResponseFormat.Base64 : StaticValues.ImageStatics.ResponseFormat.Url,
            User = "TestUser"
        });


        if (imageResult.Successful)
        {
            if (base64)
            {
                System.Console.WriteLine(imageResult.Results.Select(r => r.B64));
                return imageResult.Results.Select(r => r.B64).First();
            }
            else
            {
                System.Console.WriteLine(string.Join("\n", imageResult.Results.Select(r => r.Url)));
                return string.Join("\n", imageResult.Results.Select(r => r.Url));
            }
        }
        return $"{imageResult?.Error?.Type}: {imageResult?.Error?.Message}";
    }

    public async Task<string?> DALLE_CreateImageEdit(string prompt = "镭射猫眼", bool base64 = true)
    {
        //DALL·E Sample
        var imageResult = await OpenAiService.Image.CreateImageEdit(new ImageEditCreateRequest
        {
            Prompt = prompt,
            N = 1,
            Size = StaticValues.ImageStatics.Size.Size512,
            ResponseFormat = base64 ? StaticValues.ImageStatics.ResponseFormat.Base64 : StaticValues.ImageStatics.ResponseFormat.Url,
            User = "TestUser"
        });


        if (imageResult.Successful)
        {
            if (base64)
            {
                System.Console.WriteLine(imageResult.Results.Select(r => r.B64));
                return imageResult.Results.Select(r => r.B64).First();
            }
            else
            {
                System.Console.WriteLine(string.Join("\n", imageResult.Results.Select(r => r.Url)));
                return string.Join("\n", imageResult.Results.Select(r => r.Url));
            }
        }
        return $"{imageResult?.Error?.Type}: {imageResult?.Error?.Message}";
    }

    public async Task<string?> DALLE_CreateImageVariation(string prompt = "镭射猫眼", bool base64 = true)
    {
        //DALL·E Sample
        var imageResult = await OpenAiService.Image.CreateImageVariation(new ImageVariationCreateRequest
        {
            ImageName = prompt,
            N = 1,
            Size = StaticValues.ImageStatics.Size.Size512,
            ResponseFormat = base64 ? StaticValues.ImageStatics.ResponseFormat.Base64 : StaticValues.ImageStatics.ResponseFormat.Url,
            User = "TestUser"
        });


        if (imageResult.Successful)
        {
            if (base64)
            {
                System.Console.WriteLine(imageResult.Results.Select(r => r.B64));
                return imageResult.Results.Select(r => r.B64).First();
            }
            else
            {
                System.Console.WriteLine(string.Join("\n", imageResult.Results.Select(r => r.Url)));
                return string.Join("\n", imageResult.Results.Select(r => r.Url));
            }
        }
        return $"{imageResult?.Error?.Type}: {imageResult?.Error?.Message}";
    }

}


/// <summary>
///
/// </summary>
public enum EnumOpenaiModdel
{
    /// <summary>
    ///ChatGPT
    /// </summary>
    [Display(Name = "ChatGPT")]
    ChatGpt,

    /// <summary>
    ///Completions
    /// </summary>
    [Display(Name = "Completions")]
    Completions,

    /// <summary>
    /// Completions Stream
    /// </summary>
    [Display(Name = "Completions Stream")]
    CompletionsStream,

    /// <summary>
    /// DALL-E
    /// </summary>
    [Display(Name = "DALL-E")]
    DALLE,
}


//CodeGPT 扩展还有几个设置，可以根据个人喜好进行配置。它们包括：

//1. Max Tokens：在 API 处理提示之前，输入被分解为标记，然后 API 处理这些标记。max tokens 是 API 应该接受和处理的最大标记数。因此，根据你想要获得的响应长度选择标记数。还要注意，每个模型（稍后会讲到更多）都有一个最大标记数，所以使用的模型会影响最大标记数。
//2. Model：这是 CodeGPT 在处理查询时将使用的 OpenAI 模型。顾名思义，CodeGPT 使用 GPT-3 模型。在这个模型中，text-davinci-003 是最有能力的，因为它能够提供更高质量、更长的输出，正确地跟随提示，并处理高达 4,000 个标记。
//3. Language：这是你将与 API 交互的语言。功能，如 Explain 或 Document，也将在所选语言中完成。（这里你可以选择中文）
//4. Temperature：此设置确定生成文本中的随机程度或“创造力”水平。温度越高，生成的输出就越多样化和有创意。较低的温度会产生类似于训练数据的输出，并且不太可能包含意外或惊人的内容。它是一个介于零（0）和一（1）之间的值，其中零表示最确定性，而一（1）表示最随机和有创造力。默认温度为 0.3。
