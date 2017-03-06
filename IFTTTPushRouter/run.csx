#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;
using ExtensionGoo.Standard.Extensions;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ICollector<LogResult> outputTable, TraceWriter log)
{
    string data = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "Text", true) == 0)
        .Value;       
    
        
    var pushUrl = "<your_ifttt_maker_url/>";
    await pushUrl.Post("{\"value1\":\" " + data + "\"}");

    var dtMax = (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks).ToString("d19");       
    
    var logEntry = new LogResult{
        LogTimeUTC = DateTime.UtcNow, 
        Text = data,
        RowKey = dtMax,
        PartitionKey = "PushLog"
    };

    outputTable.Add(logEntry);

    return req.CreateResponse(HttpStatusCode.OK, "Pushed");
}

public class LogResult : TableEntity {
    public DateTime LogTimeUTC { get;set;}
    public string Text{get;set;}
}

