using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Core.Helpers;

public static class HtmlTemplateGenerator
{
    public static string ConfirmMessage(string message, string url)
    {

        string containerStyle = @"width:300px;height:150px;background-color:#f4f4f4;
                                    border-radius:10px;margin-inline:auto;
                                    display:flex;justify-content:center;align-items:center;
                                    flex-direction:column";

        string buttonStyle = @"background-color:#00a8e8;padding:12px 16px;border-radius:8px;color:white;cursor:pointer;text-decoration:none";



        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine($"<div style='{containerStyle}'>");
        stringBuilder.AppendLine($"    <p>{message}</p>");
        stringBuilder.AppendLine($"    <a href='{url}' style='{buttonStyle}'>Confirm Code</a>");
        stringBuilder.AppendLine("</div>");

        return stringBuilder.ToString();
    }

}
