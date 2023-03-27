// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using System.ComponentModel.DataAnnotations;

namespace BootstrapBlazor.OpenAI.GPT3.Services;

/// <summary>
///
/// </summary>
public enum EnumOpenAiModel
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

    ///// <summary>
    ///// Completions Stream
    ///// </summary>
    //[Display(Name = "Completions Stream")]
    //CompletionsStream,

    /// <summary>
    /// DALL-E
    /// </summary>
    [Display(Name = "DALL-E")]
    DALLE,
}
