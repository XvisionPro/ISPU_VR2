using System;
using System.Collections.Generic;
using SimpleJSON;

public class Query
{
    public string entryPoint { get; private set; }
    public Dictionary<string, string> getParams { get; private set; }
    public Dictionary<string, string> postParams { get; private set; }
    public Action<JSONNode> onComplete{ get; private set; }
    public Action<string> onError { get; private set; }
    public int iteration { get; private set; }
    public bool toSN { get; private set; }

    public Query(string entryPoint, Dictionary<string, string> getParams = null, Dictionary<string, string> postParams = null, Action<JSONNode> onComplete = null, Action<string> onError = null)
    {
        this.entryPoint = entryPoint;
        this.getParams = getParams;
        this.postParams = postParams;
        this.onComplete = onComplete;
        this.onError = onError;
    }

    public void onErrorIteration()
    {
        iteration++;
    }
    
    public void setIteration(int val)
    {
        iteration = val;
    }
    public void setToSN(bool val)
    {
        toSN = val;
    }
    
}