#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
public class PlayerMovement : Form
{
    private int speed = 10;
    private int circleSize = 50;
    private bool up, down, left, right;

    private int playAreaHeight;
    private int playerX, playerY;

    private System.Windows.Forms.Timer gameloop;
    private System.Windows.Forms.Timer aniTimer;
    private Image idleAni;
    private List<Image> walkingFrames = new List<Image>();
    private bool playerIsMoving = false;
    private int frameIndex = 0;

    public PlayerMovement()
    {
        this.FormBorderStyle = FormBorderStyle.None; // Remove window borders
        this.WindowState = FormWindowState.Maximized;
        this.DoubleBuffered = true; // Reduce flickering

        string animationFolder = @"C:\Users\wugg\Downloads\Major_Project\Final_Design_Animation";
        string idlePath = Path.Combine(animationFolder, "Gavin Temporary Pose (Idle).png");
        string walkingPath = Path.Combine(animationFolder, "Gavin Final Animation (Walking).gif");

        if (!File.Exists(idlePath) || !File.Exists(walkingPath))
        {
            MessageBox.Show("Animation files not found! Please check the file paths.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            return;
        }

        idleAni = Image.FromFile(idlePath);
        LoadWalkingAnimation(walkingPath);

        gameloop = new System.Windows.Forms.Timer();
        gameloop.Interval = 16; // ~60 FPS
        gameloop.Tick += GameLoop_Tick;
        gameloop.Start();

        aniTimer = new System.Windows.Forms.Timer();
        aniTimer.Interval = 100; // Adjust animation speed
        aniTimer.Tick += UpdateAnimation;
        aniTimer.Start();

        this.Text = "Player Movement";
        this.KeyPreview = true;
        this.Paint += DrawPlayer;
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;

        playAreaHeight = this.Height / 4;
        //starting position of the player
        playerX = this.Width / 2 - circleSize / 2;
        playerY = this.Height - playAreaHeight - circleSize;
    }

    private void GameLoop_Tick(object sender, EventArgs e)
    {
        Loop();
        Invalidate();
    }

    private void UpdateAnimation(object sender, EventArgs e)
    {
        if (playerIsMoving && walkingFrames.Count > 0)
        {
            frameIndex = (frameIndex + 1) % walkingFrames.Count;
        }
        else
        {
            frameIndex = 0;
        }
        Invalidate();
    }

    private void LoadWalkingAnimation(string filePath)
    {
        Image gif = Image.FromFile(filePath);
        FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
        int frameCount = gif.GetFrameCount(dimension);

        for (int i = 0; i < frameCount; i++)
        {
            gif.SelectActiveFrame(dimension, i);
            walkingFrames.Add(new Bitmap(gif)); // Clone the frame
        }

        gif.Dispose(); // Dispose after frames are extracted
    }

    private void DrawPlayer(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Image currentSprite = (playerIsMoving && walkingFrames.Count > 0) ? walkingFrames[frameIndex] : idleAni;
        g.DrawImage(currentSprite, new Rectangle(playerX, playerY, circleSize, circleSize));
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W)
        { 
            up = true;
        }
        if (e.KeyCode == Keys.S)
        { 
            down = true;
        }
        if (e.KeyCode == Keys.A)
        { 
            left = true;
        }
        if (e.KeyCode == Keys.D)
        { 
            right = true;
        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W)
        { 
            up = false;
        }
        if (e.KeyCode == Keys.S)
        { 
            down = false;
        }
        if (e.KeyCode == Keys.A)
        { 
            left = false;
        }
        if (e.KeyCode == Keys.D)
        { 
            right = false;
        }
    }

    private void Loop()
    {
        int moveX = playerX;
        int moveY = playerY;

        if (up)
        { 
            moveY -= speed;
        }
        if (down)
        { 
            moveY += speed;
        }
        if (left)
        { 
            moveX -= speed;
        }
        if (right)
        { 
            moveX += speed;
        }

        moveY = Math.Max(this.Height - playAreaHeight - circleSize, Math.Min(moveY, this.Height - circleSize));
        moveX = Math.Max(0, Math.Min(moveX, this.Width - circleSize));

        playerX = moveX;
        playerY = moveY;
        playerIsMoving = up || down || left || right;
    }
}