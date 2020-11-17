using Godot;
using System;
using EvilFarmingGame.Items;

public class Dialogue : CanvasLayer
{
    public string[] DialogueFiles;
    public string FacesFile;
    public int DialogueAmount;
    public Player PlayerBody;

    public Control ControlNode;
    private RichTextLabel Label;
    private RichTextLabel NameLabel;

    public bool IsShown = false;
    public int Page = 0;
    public int CurrentFile = 0;
    public int AmountInFile;
    private dynamic CurrentDialogue;
    
    private enum State {
        Dialogue = 0, 
        Announcement
    }

    private State CurrentState;

    public override void _Ready()
    {
        ControlNode = GetNode<Control>("Control");
        NameLabel = ControlNode.GetNode<RichTextLabel>("Name");
        Label = ControlNode.GetNode<RichTextLabel>("RichTextLabel");
    }

    public override void _Process(float delta)
    {
        if (IsShown)
        {
            if (PlayerBody != null)
            {
                PlayerBody.UI.Hide();
                PlayerBody.CanMove = false;
            }
            ControlNode.Visible = true;
            
        }
        else
        {
            if (PlayerBody != null)
            {
                PlayerBody.CanMove = true;
            }
            ControlNode.Visible = false;
        }
    }

    public void Show()
    {
        CurrentState = State.Dialogue;
        CurrentDialogue = ReadJson(DialogueFiles[CurrentFile])["Dialogue"][Page.ToString()]["Text"];
        NameLabel.Text = ReadJson(DialogueFiles[CurrentFile])["Name"];
        AmountInFile = (int)ReadJson(DialogueFiles[CurrentFile])["Amount"];
        DisplayDialogue(CurrentDialogue);
    }

    public void Announce(string Announcement)
    {
        CurrentState = State.Announcement;
        CurrentDialogue = Announcement;
        NameLabel.Text = "";
        AmountInFile = 0;
        DisplayDialogue(CurrentDialogue);
    }

    private void LabelTimout()
    {
        Label.VisibleCharacters++;
    }

    private void DisplayDialogue(dynamic what)
    {
        Label.VisibleCharacters = 0;
        Label.BbcodeText = what.ToString();
        IsShown = true;
        if(PlayerBody != null) PlayerBody.CanMove = false;
    }
    
    public static dynamic ReadJson(string path)
    {
        var file = new File();
        try {
            file.Open(path, File.ModeFlags.Read);
        }
        catch { throw new Exception("Invalid JSON-file"); }
        if (file.IsOpen())
        {
            string jsonText = file.GetAsText();
            return JSON.Parse(jsonText).Result;
        }
        return null;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Player_Action") && IsShown)
        {
            if (Label.VisibleCharacters < ((string) CurrentDialogue).Length)
            {
                Label.VisibleCharacters = ((string) CurrentDialogue).Length;
            }
            else if (Page < AmountInFile)
            {
                Page++;
                Show();
            }
            else
            {
                IsShown = false;
                try {
                    PlayerBody.Inventory.Gain(Database<Item>.Get(
                        ReadJson(DialogueFiles[CurrentFile])["Item"]));
                }
                catch
                {
                    PlayerBody.UI.Visible = true;
                }

            }
        }
    }
}
