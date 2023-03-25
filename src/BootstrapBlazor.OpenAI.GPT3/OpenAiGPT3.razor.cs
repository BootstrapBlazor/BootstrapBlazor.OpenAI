// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazor.Components;

/// <summary>
/// Blazor OpenAI GPT3 组件 
/// </summary>
public partial class OpenAiGPT3 : IAsyncDisposable
{
    [Inject]
    [NotNull]
    private IJSRuntime? JSRuntime { get; set; }

    [NotNull]
    private IJSObjectReference? Module { get; set; }

    private ElementReference Element { get; set; }

    private string ID { get; set; }=Guid.NewGuid().ToString("N");

    [NotNull]
    [Inject]
     private  IConfiguration? Config { get; set; }

    [NotNull]
    [Inject]
     private OpenAiBBService? OpenaiService { get; set; }

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

    private string? PlaceHolderText { get; set; }="问点啥,可选模型后再问我.";

    private int Lines { get; set; } = 0;

    [NotNull]
    private EnumOpenaiModdel? SelectedEnumItem { get; set; }

    [Parameter]
    public string? OpenAIKey { get; set; }

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
            Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BootstrapBlazor.OpenAI.GPT3/app.js" + "?v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        }
    }


    private async Task OnEnterAsync(string val)
    {
        if (string.IsNullOrWhiteSpace (val))
        {
            return;
        } 

        Lines++;
        if (Lines > 20)
        {
            ResultText=string.Empty;
            Lines = 1;
        }
        ResultText+=($"Q: {val}{Environment.NewLine}");
        InputText = string.Empty;
        PlaceHolderText = "思考中...";
        ResultImage = null;
        string? res;
        switch (SelectedEnumItem)
        {
            default:
                ResultText += "[ChatGpt]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.ChatGPT(val);
                break;
            case EnumOpenaiModdel.Completions:
                ResultText += "[Completions]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.Completions(val);
                break;
            //case EnumOpenaiModdel.CompletionsStream:
            //    ResultText += "[Completions Stream]" + Environment.NewLine;
            //    await UpdateUI();
            //    res = await OpenaiService.CompletionsStream(val);
            //    break;
            case EnumOpenaiModdel.DALLE:
                ResultText += "[DALL·E]" + Environment.NewLine;
                await UpdateUI();
                res = await OpenaiService.DALLE_CreateImage(val);
                if (res != null)
                {
                    var imageDataUrl = $"data:image/jpg;base64,{res}";
                    ResultImage = imageDataUrl;
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
                ResultText+=(Environment.NewLine);
            }
            else if (res != string.Empty)
            {
                ResultText+=($"A: {res}{Environment.NewLine}");
            }
            ResultText+=(Environment.NewLine);
            InputText = string.Empty;
            await UpdateUI();
            PlaceHolderText = "问点啥,可选模型后再问我.";
        }
    }

    /// <summary>
    /// 更新界面以及自动滚动
    /// </summary>
    /// <param name="scroll"></param>
    /// <returns></returns>
    private async Task UpdateUI(bool scroll=true)
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

    private  Task OnClear()
    {
        ResultText=string.Empty;
        InputText=string.Empty;
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




