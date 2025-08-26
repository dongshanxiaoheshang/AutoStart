using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Text.RegularExpressions;

public class TaskManager : MonoBehaviour
{
    List<Task> tasks = new List<Task>();

    //  public Text DiaText;
    public TextMeshProUGUI play;
    public TextMeshProUGUI npc;
    public string[] dias;
    public int currentIndex = 0;

    private void Start()
    {
        TextAsset dialogAsset = Resources.Load<TextAsset>("dialog");
        //  dias = dialogAsset.text.Split('\n');

        //for (int i = 0; i < dias.Length; i++)
        //{
        //    dias[i] = Regex.Replace(dias[i], @"^\S+: ", "");
        //}
        if (dialogAsset != null)
        {
            dias = dialogAsset.text.Split('\n');
            play.text = Regex.Replace(dias[currentIndex], @"^\S+: ", "");
        }

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextDia();
        }
    }

    public void NextDia()
    {
        //if (currentIndex < dias.Length - 1)
        //{
        //    currentIndex++;
        //    TextMeshProUGUI.text = dias[currentIndex];
        //}
        //print(TextMeshProUGUI.text);

        if (currentIndex < dias.Length)
        {
            if (dias[currentIndex].StartsWith("角色1"))
            {
                play.text = "";
                play.text = Regex.Replace(dias[currentIndex], @"^\S+: ", "");
                //玩家对话框显示，其他对话框隐藏                
            }
            else
            {
                npc.text = "";
                npc.text = Regex.Replace(dias[currentIndex], @"^\S+: ", "");
            }
        }
        currentIndex++;
    }




    public void AddTask(string taskName, string taskDrection)
    {
        Task task = new Task(taskName, taskDrection);
        tasks.Add(task);
    }

    public void RemoveTask(string taskName)
    {
        Task newTask = tasks.Find(t => t.TaskName == taskName);
        if (newTask != null)
            tasks.Remove(newTask);
        else
            print("wadawdada");
    }
}
