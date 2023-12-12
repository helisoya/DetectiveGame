using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A CaseAdapter is a mean to more efficiently process data from the SaveFile. It can be used to check if an evidence was found faster.
/// </summary>
public class CaseAdapter
{
    public Dictionary<string, bool> evidenceFound;
    public CaseAdapter()
    {
        evidenceFound = new Dictionary<string, bool>();
    }
}
