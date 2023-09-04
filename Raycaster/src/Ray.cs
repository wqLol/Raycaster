using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Raycaster;
public class Ray {
    public static int res = 10;
    public static List<Ray> rays = new List<Ray>();
    public static float speed = 0.001f;
    public float Rotation;
    private int mmssx;
    private int mmssy; 
    public Vector2 Pos;
    private Texture2D _texture;
    public bool canMove = true;
    public float x;
    public Ray(float dir,Vector2 Position,Texture2D texture,float xpos){
        Rotation = dir;
        Pos = Position;
        mmssy = 400/World.world.GetLength(1);
        mmssx = 400/(World.world.GetLength(0)-1);
        _texture =texture;
        x = xpos;
        rays.Add(this);
    }
    public void Draw(SpriteBatch spriteBatch){
        spriteBatch.Draw(_texture,Pos,Color.Blue);
    }
    public void Move(){
        Vector2 movement = Vector2.Zero; 
        if (canMove){
            while (canMove){
                movement.X += (float)Math.Cos(MathHelper.ToDegrees(Rotation)) * Ray.speed;
                movement.Y += (float)Math.Sin(MathHelper.ToDegrees(Rotation))* Ray.speed ;
                checkCollision(movement);
                if (!canMove) {
                    break;
                }
            }
        }
    }
    public bool checkCollision(Vector2 movement){
        Vector2 newPos = Pos + movement;
        int squarey =(int)Math.Floor(newPos.Y/mmssy);
        int squarex = (int)Math.Floor(newPos.X/mmssx);
        // Console.WriteLine(World.world[squarey,squarex]);

        try {
            if (World.world[squarey,squarex] == 1){
                canMove = false;
                return true;
            }
        } catch {}
        Pos = newPos;
        return false;
    }
    public static void shootRay(float dir, Vector2 Position, Texture2D texture)
{
     // 5-degree difference in radians
    int FOV = 60;
    int nofRays = 120;
    float angleInc = MathHelper.ToRadians(nofRays/FOV * 0.001f);
    float x = 0;
    float offset = MathHelper.ToRadians(-45.5F);
    dir-=FOV/4;
    for (int i = 0;i<nofRays*4;i++){
        new Ray((dir + (angleInc*i))-offset,Position,texture,x);
        x+=Ray.res/2;
    }    

} public void Cast(Player player,LineDrawer lineDrawer){
    float distance = Vector2.Distance(Pos,player.Pos);
    
    // Console.WriteLine(Math.Cos(player.Rotation-Rotation));
    float height = (mmssx*1080)/distance *(float)Math.Cos(player.Rotation-Rotation) *13; if (height>1080) {height=1080;}

    float lineOffset = (600-height/2) -(Camera.OffsetY*5);

    int col = (int)((255/distance*25)-(255 * Math.Floor(255/distance*5/255)))*2;
    // Console.WriteLine(col);
    lineDrawer.DrawLine(new Vector2(x,lineOffset),new Vector2(x,height+lineOffset),new Color(col/5,col/5,0,50),Ray.res);

}
}