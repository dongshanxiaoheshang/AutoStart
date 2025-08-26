using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public string TaskName { get; private set; }
    public string TaskDescription { get; private set; }


    public Task(string taskName, string taskDescription)
    {
        this.TaskName = taskName;
        this.TaskDescription = taskDescription;
    }
}
