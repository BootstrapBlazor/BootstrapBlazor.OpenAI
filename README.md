# Blazor OpenAI 组件  

![image](https://user-images.githubusercontent.com/8428709/233230898-46dec124-cc61-4ffc-ba52-bd96da6e2b04.png)


示例:

https://www.blazor.zone/OpenAI

https://blazor.app1.es/OpenAI

使用方法:

1. nuget包

    ```
    BootstrapBlazor.OpenAI
    ```

2. _Imports.razor 文件 或者页面添加 添加组件库引用

    ```
    @using BootstrapBlazor.Components
    ```

3. Program.cs 文件添加

    ```
    builder.Services.AddTransient<OpenAiClientService>();
    ```

4. Key

    `appsettings.json`或者其他配置文件添加配置

    ```
    "OpenAIKey": "OpenAIKey"
    ```

5. Razor页面

    ```
    <OpenAiGPT3 />
    ```

4.配置文件参数说明 


|  参数   | 说明  |
|  ----  | ----  |
| OpenAIKey  | OpenAIKey |
| AzureOpenAIUrl  | AzureOpenAI Endpoint, 配置后使用AzureOpenAI |
| AzureOpenAIKey  | AzureOpenAI Key | 

---
 

---
#### Blazor 组件

[条码扫描 ZXingBlazor](https://www.nuget.org/packages/ZXingBlazor#readme-body-tab)
[![nuget](https://img.shields.io/nuget/v/ZXingBlazor.svg?style=flat-square)](https://www.nuget.org/packages/ZXingBlazor) 
[![stats](https://img.shields.io/nuget/dt/ZXingBlazor.svg?style=flat-square)](https://www.nuget.org/stats/packages/ZXingBlazor?groupby=Version)

[图片浏览器 Viewer](https://www.nuget.org/packages/BootstrapBlazor.Viewer#readme-body-tab)
  
[条码扫描 BarcodeScanner](Densen.Component.Blazor/BarcodeScanner.md)
   
[手写签名 Handwritten](Densen.Component.Blazor/Handwritten.md)

[手写签名 SignaturePad](https://www.nuget.org/packages/BootstrapBlazor.SignaturePad#readme-body-tab)

[定位/持续定位 Geolocation](https://www.nuget.org/packages/BootstrapBlazor.Geolocation#readme-body-tab)

[屏幕键盘 OnScreenKeyboard](https://www.nuget.org/packages/BootstrapBlazor.OnScreenKeyboard#readme-body-tab)

[百度地图 BaiduMap](https://www.nuget.org/packages/BootstrapBlazor.BaiduMap#readme-body-tab)

[谷歌地图 GoogleMap](https://www.nuget.org/packages/BootstrapBlazor.Maps#readme-body-tab)

[蓝牙和打印 Bluetooth](https://www.nuget.org/packages/BootstrapBlazor.Bluetooth#readme-body-tab)

[PDF阅读器 OpenAI.GPT](https://www.nuget.org/packages/BootstrapBlazor.OpenAI.GPT#readme-body-tab)

[文件系统访问 FileSystem](https://www.nuget.org/packages/BootstrapBlazor.FileSystem#readme-body-tab)

[光学字符识别 OCR](https://www.nuget.org/packages/BootstrapBlazor.OCR#readme-body-tab)

[电池信息/网络信息 WebAPI](https://www.nuget.org/packages/BootstrapBlazor.WebAPI#readme-body-tab)

[视频播放器 VideoPlayer](https://www.nuget.org/packages/BootstrapBlazor.VideoPlayer#readme-body-tab)

#### AlexChow

[今日头条](https://www.toutiao.com/c/user/token/MS4wLjABAAAAGMBzlmgJx0rytwH08AEEY8F0wIVXB2soJXXdUP3ohAE/?) | [博客园](https://www.cnblogs.com/densen2014) | [知乎](https://www.zhihu.com/people/alex-chow-54) | [Gitee](https://gitee.com/densen2014) | [GitHub](https://github.com/densen2014)
