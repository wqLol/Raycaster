using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Raycaster;
public class Player
{
    public Texture2D _texture;
    public Vector2 Pos;
    private float _rotation;
    public float Rotation
        {
            get
            {
                // Return the sum of internal rotation and Camera.OffsetX
                try{
                    return _rotation + (Camera.OffsetX*0.0001f);
                } catch{return _rotation; }
            }
            set
            {
                // Set the internal rotation value directly
                _rotation = value;
            }
        }
    // public float Rotation {get;set;}
    public Player(Texture2D t, Vector2 pos) {
        mmssy = 400/World.world.GetLength(1);
        mmssx = 400/(World.world.GetLength(0)-1);
        Pos = pos;
        _texture = t;
    }
    private int mmssx;
    private int mmssy; 
    

    public void Update(){
        KeyboardState keystate = Keyboard.GetState();
        Vector2 movement = Vector2.Zero;
        if (keystate.IsKeyDown(Keys.S)){
            movement.X += (float)Math.Cos(MathHelper.ToDegrees(Rotation));
            movement.Y += (float)Math.Sin(MathHelper.ToDegrees(Rotation));
        }
        if (keystate.IsKeyDown(Keys.W)){
            movement.X  -= (float)Math.Cos(MathHelper.ToDegrees(Rotation));
            movement.Y -= (float)Math.Sin(MathHelper.ToDegrees(Rotation));
        }
        if (keystate.IsKeyDown(Keys.A)){
            movement.X += (float)Math.Cos(MathHelper.ToDegrees(Rotation)+90)*0.3f;
            movement.Y += (float)Math.Sin(MathHelper.ToDegrees(Rotation)+90)*0.3f;
        }
        if (keystate.IsKeyDown(Keys.D)){
            movement.X  += (float)Math.Cos(MathHelper.ToDegrees(Rotation)-90) *0.3f;
            movement.Y += (float)Math.Sin(MathHelper.ToDegrees(Rotation)-90)*0.3f;
        }
        if (keystate.IsKeyDown(Keys.Right)){
            Rotation+=0.0005f;
        }
        if (keystate.IsKeyDown(Keys.Left)){
            Rotation-=0.0005f;
        }
        
        // Rotation = (float)Math.Floor(Camera.OffsetX) + Rotation;
        // Console.WriteLine((float)Math.Floor(Camera.OffsetX));
        checkCollision(movement * 0.2f);
    } 
    public void checkCollision(Vector2 movement){
        Vector2 newPos = Pos + movement;
        int squarey =(int)Math.Floor(newPos.Y/mmssy);
        int squarex = (int)Math.Floor(newPos.X/mmssx);
        // Console.WriteLine(World.world[squarey,squarex]);

        
        if (World.world[squarey,squarex] == 1){
            newPos -= movement;
        }
        Pos = newPos;
    }
    public void Draw(SpriteBatch spriteBatch){
        spriteBatch.Draw(_texture, Pos,null, Color.Blue,MathHelper.ToDegrees(Rotation),new Vector2(_texture.Width/2,_texture.Height/2),1,SpriteEffects.None,0f);
    }
}