// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using BootstrapBlazor.OpenAI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazor.Components;

/// <summary>
/// Blazor OpenAI 组件 
/// </summary>
public partial class OpenAI : IAsyncDisposable
{
    [Inject]
    [NotNull]
    private IJSRuntime? JSRuntime { get; set; }

    [NotNull]
    private IJSObjectReference? Module { get; set; }

    private ElementReference Element { get; set; }

    private string ID { get; set; } = Guid.NewGuid().ToString("N");

    [NotNull]
    [Inject]
    private IConfiguration? Config { get; set; }

    [NotNull]
    [Inject]
    private OpenAiClientService? OpenaiService { get; set; }

    /// <summary>
    /// 获得/设置 查询关键字
    /// </summary>
    [Parameter]
    public string? Search { get; set; }

    string? ErrorMessage { get; set; }

    [DisplayName("问点啥")]
    private string? InputText { get; set; }

    private string? ResultText { get; set; }
    private string? ResultImage { get; set; }

    private string? PlaceHolderText { get; set; } = "问点啥,可选模型后再问我.";

    private int Lines { get; set; } = 0;

    [NotNull]
    private EnumOpenAiModel? SelectedEnumItem { get; set; }

    [NotNull]
    private IEnumerable<SelectedItem> ItemsMaxToken { get; set; } = new[] {
        new SelectedItem("5", "5"),
        new SelectedItem("20", "20"),
        new SelectedItem("100", "100"),
        new SelectedItem("300", "300"),
        new SelectedItem("500", "500"),
        new SelectedItem("2000", "2000"),
        new SelectedItem("3000", "3000"),
        new SelectedItem("4000", "4000"),
        new SelectedItem("5000", "5000"),
        new SelectedItem("10000", "10000"),
        new SelectedItem("20000", "20000"),
    };

    int SelectedMaxTokens { get; set; } = 500;

    [NotNull]
    private IEnumerable<SelectedItem> ItemsTemperature { get; set; } = new[] {
        new SelectedItem("0.1", "0.1"),
        new SelectedItem("0.2", "0.2"),
        new SelectedItem("0.5", "0.5"),
        new SelectedItem("0.6", "0.6"),
        new SelectedItem("0.7", "0.7"),
        new SelectedItem("0.8", "0.8"),
        new SelectedItem("0.9", "0.9"),
    };

    float SelectedTemperature { get; set; } = 0.5f;

    [Parameter]
    public string? OpenAIKey { get; set; }

    [Parameter]
    public bool ShowOptions { get; set; } = true;

    /// <summary>
    /// 完成时生成的最大令牌数。默认值为 500<para></para>参数为空, 内置 SelectedMaxTokens 优先
    /// </summary>
    [Parameter]
    public int? MaxTokens { get; set; }

    /// <summary>
    /// 浮点数，控制模型的输出的多样性。值越高，输出越多样化。值越低，输出越简单。默认值为 0.5<para></para>参数为空, 内置 SelectedTemperature 优先
    /// </summary>
    [Parameter]
    public float? Temperature { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.TryGetValue<string>(nameof(OpenAIKey), out var value))
        {
            OpenAIKey = value ?? Config["OpenAIKey"];
            OpenaiService.Init(OpenAIKey);
        }

        await base.SetParametersAsync(parameters);
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.OpenAI/app.js" + "?v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        }
    }

    private async Task OnEnter()
    {
        var val = InputText;
        if (string.IsNullOrWhiteSpace(val))
        {
            return;
        }

        Lines++;
        if (Lines > 20)
        {
            ResultText = string.Empty;
            Lines = 1;
        }
        ResultText += ($"Q: {val}{Environment.NewLine}");
        InputText = string.Empty;
        PlaceHolderText = "思考中...";
        ResultImage = null;
        string? res;
        switch (SelectedEnumItem)
        {
            default:
                ResultText += "[ChatGpt]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.ChatGPT(val, MaxTokens ?? SelectedMaxTokens, Temperature ?? SelectedTemperature);
                break;
            case EnumOpenAiModel.ChatGpt4:
                ResultText += "[ChatGpt4]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.ChatGPT(val, MaxTokens ?? SelectedMaxTokens, Temperature ?? SelectedTemperature, model: "gpt4");
                break;
            case EnumOpenAiModel.ChatGpt4_32k:
                ResultText += "[ChatGpt4 32k]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.ChatGPT(val, MaxTokens ?? SelectedMaxTokens, Temperature ?? SelectedTemperature, model: "gpt4-32k");
                break;
            case EnumOpenAiModel.ChatGptAiHomeAssistant:
                ResultText += "[AiHomeAssistant]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.ChatGPT(val, MaxTokens ?? SelectedMaxTokens, Temperature ?? SelectedTemperature, true);
                break;
            case EnumOpenAiModel.Completions:
                ResultText += "[Completions]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.Completions(val, MaxTokens ?? SelectedMaxTokens, Temperature ?? SelectedTemperature);
                break;
            case EnumOpenAiModel.NaturalLanguageToSQL:
                ResultText += "[NaturalLanguageToSQL]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.NaturalLanguageToSQL(val);
                break;
            case EnumOpenAiModel.Chatbot:
                ResultText += "[Chatbot]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.Chatbot(val);
                break;
            case EnumOpenAiModel.ExtractingInformation:
                ResultText += "[ExtractingInformation]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.ExtractingInformation(val);
                break;
            //case EnumOpenaiModdel.CompletionsStream:
            //    ResultText += "[Completions Stream]" + Environment.NewLine;
            //    await UpdateUI();
            //    res = await OpenaiService.CompletionsStream(val);
            //    break;
            case EnumOpenAiModel.DALLE:
                ResultText += "[DALL·E]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.DALLE_CreateImage(val, false);
                if (res != null)
                {
                    if (res.StartsWith("http"))
                    {
                        var httpclient = new HttpClient();
                        var stream = await httpclient.GetStreamAsync(res);
                        ResultImage = res;
                    }
                    else
                    {
                        var imageDataUrl = $"data:image/jpg;base64,{res}";
                        ResultImage = imageDataUrl;
                    }
                    ResultText += Environment.NewLine;
                    res = string.Empty;
                }
                break;
        }

        if (res != null)
        {
            if (res.StartsWith("http"))
            {
                var httpclient = new HttpClient();
                var stream = await httpclient.GetStreamAsync(res);

                ResultImage = res;
                ResultText += (Environment.NewLine);
            }
            else if (res != string.Empty)
            {
                ResultText += ($"A: {res}{Environment.NewLine}");
            }
            ResultText += (Environment.NewLine);
            InputText = string.Empty;
            PlaceHolderText = "问点啥,可选模型后再问我.";
        }
        else
        {
            PlaceHolderText = "AI开小差了. 重新问点啥吧,可选模型后再问我.";
        }
        await UpdateUI();
    }

    private async Task OnEnterAsync(string val)
    {
        await OnEnter();
    }

    /// <summary>
    /// 更新界面以及自动滚动
    /// </summary>
    /// <param name="scroll"></param>
    /// <returns></returns>
    private async Task UpdateUI(bool scroll = true)
    {
        StateHasChanged();
        if (!scroll) return;
        //await Module!.InvokeVoidAsync("AutoScrollTextarea", Element);
        await Module!.InvokeVoidAsync("AutoScrollTextareaByID", ID);
    }

    private Task OnEscAsync(string val)
    {
        InputText = string.Empty;
        return Task.CompletedTask;
    }

    private Task OnClear()
    {
        ResultText = string.Empty;
        InputText = string.Empty;
        ResultImage = null;
        return Task.CompletedTask;
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        if (Module is not null)
        {
            await Module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }


}




