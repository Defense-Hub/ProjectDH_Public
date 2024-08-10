using System.Collections.Generic;
using GoogleSheet.Type;
using UnityEngine;

[Type(Type: typeof(SplashData), TypeName: new string[] { "SplashData" })]
public class SplashDataReader : IType
{
    public object DefaultValue => null;
    public object Read(string value)
    {
        string[] split = value.Split(',');
        return new SplashData()
        {
            Probabillity = int.Parse(split[0]),
            SplashRange =  float.Parse(split[1])
        };
    }

    public string Write(object value)
    {
        return null;
    }
}

[Type(Type: typeof(StunData), TypeName: new string[] { "StunData" })]
public class StunDataReader : IType
{
    public object DefaultValue => null;
    public object Read(string value)
    {
        string[] split = value.Split(',');
        return new StunData()
        {
            Probabillity = int.Parse(split[0]),
            EffectDuration =  float.Parse(split[1])
        };
    }

    public string Write(object value)
    {
        return null;
    }
}

[Type(Type: typeof(SlowData), TypeName: new string[] { "SlowData" })]
public class SlowDataReader : IType
{
    public object DefaultValue => null;
    public object Read(string value)
    {
        string[] split = value.Split(',');
        return new SlowData()
        {
            Probabillity = int.Parse(split[0]),
            Delta =   float.Parse(split[1]),
            EffectDuration =   float.Parse(split[2]),
        };
    }

    public string Write(object value)
    {
        return null;
    }
}