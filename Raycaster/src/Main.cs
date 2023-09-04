using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
namespace Raycaster;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private int[,] world = World.world;
    private int mmssx;
    private int mmssy; 
    private Texture2D emptyWhiteTexture;
    private Texture2D playerTexture;
    private Color outlineColor = Color.Black; 
    private bool drawMinimap = !true;
    public Texture2D RayTexture;
    public LineDrawer lineDrawer;
    private int outlineThickness = 2;
    private Texture2D Hand;
    
    private Texture2D _backgroundTexture;
    MouseState _previousMouseState;
    Player player;
    public Main()
    {
        mmssy = 400/world.GetLength(1);
        mmssx = 400/(World.world.GetLength(0)-1);
        
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        lineDrawer = new LineDrawer(GraphicsDevice);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _backgroundTexture = Content.Load<Texture2D>("backrooms");
        Hand = Content.Load<Texture2D>("hand");
        emptyWhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
        Color[] data = { Color.White };
        emptyWhiteTexture.SetData(data);
        playerTexture = new Texture2D(GraphicsDevice, 10, 10);
        Color[] playerData = new Color[10 * 10];
        for (int i = 0; i < playerData.Length; i++)
        {
            playerData[i] = Color.Pink;
        }
        playerTexture.SetData(playerData);
        // RAY TEXTURE

        RayTexture = new Texture2D(GraphicsDevice, 1, 1);
        Color[] rayTextureData = new Color[1 * 1]; // Change the variable name here
        for (int i = 0; i < rayTextureData.Length; i++)
        {
            rayTextureData[i] = Color.Black;
        }
        RayTexture.SetData(rayTextureData);

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        player = new Player(playerTexture, new Vector2(mmssx*World.world.GetLength(0)/2,mmssx*World.world.GetLength(1)/2));
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        
        try{
        MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.X>620 && currentMouseState.X < 1100){
            // Calculate the mouse movement
            float deltaX = currentMouseState.X - _previousMouseState.X;
            float deltaY = currentMouseState.Y - _previousMouseState.Y;

            // Update the camera's offset based on mouse movement
            Camera.OffsetX += deltaX * 0.1f; // Adjust the sensitivity as needed
            Camera.OffsetY += deltaY * 0.1f;
            Console.WriteLine(currentMouseState.X);
            // Update the camera
            // Camera.Update();
            
            // Reset the mouse position to the center of the screen
            
            }
            Mouse.SetPosition(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _previousMouseState = Mouse.GetState();
        } catch{}
        // Console.WriteLine(Camera.OffsetX);
        // Console.WriteLine(Camera.OffsetY);
        KeyboardState keystate = Keyboard.GetState();
        if (keystate.IsKeyDown(Keys.Escape)){
            Exit();
        }
        if (keystate.IsKeyDown(Keys.Tab)){
            World.world = World.RandomizeMap(new Random());
        }
        Ray.rays = new List<Ray>();
        Ray.shootRay(player.Rotation, player.Pos,RayTexture);
        player.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // 
        _spriteBatch.Begin();
        _spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);
        _spriteBatch.End();
        _spriteBatch.Begin();

        // MINIMAP
        if (drawMinimap){
        for(int x = 0;x<world.GetLength(0);x++){
            for(int y = 0;y<world.GetLength(1);y++){
                if (world[x,y] == 0){
                    _spriteBatch.Draw(emptyWhiteTexture,new Rectangle(y*mmssx,x*mmssy,mmssx,mmssy),Color.White);
                } else {
                    _spriteBatch.Draw(emptyWhiteTexture,new Rectangle(y*mmssx,x*mmssy,mmssx,mmssy),Color.Red);
                }
                 // You can adjust the thickness of the outline
                // _spriteBatch.Draw(emptyWhiteTexture, new Rectangle(y * mmssy, x * mmssx, mmssy, outlineThickness), outlineColor); // Top outline
                // _spriteBatch.Draw(emptyWhiteTexture, new Rectangle(y * mmssy, x * mmssx, outlineThickness, mmssx), outlineColor); // Left outline
                // _spriteBatch.Draw(emptyWhiteTexture, new Rectangle((y + 1) * mmssy - outlineThickness, x * mmssx, outlineThickness, mmssx), outlineColor); // Right outline
                // _spriteBatch.Draw(emptyWhiteTexture, new Rectangle(y * mmssy, (x + 1) * mmssx - outlineThickness, mmssy, outlineThickness), outlineColor); 
                
        };
        }
        //PLAYER
        player.Draw(_spriteBatch);
        }
        //Rays
        foreach(Ray ray in Ray.rays) {
            ray.Move();
            ray.Draw(_spriteBatch);
            ray.Cast(player,lineDrawer);
            
        }
        
        // lineDrawer.DrawLine(new Vector2(400,300), new Vector2(800,600), Color.Red);
        _spriteBatch.Draw(Hand, new Vector2(1920-Hand.Width, 1080-Hand.Height), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
