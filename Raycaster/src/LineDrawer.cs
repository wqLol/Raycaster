using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LineDrawer
{
    private GraphicsDevice graphicsDevice;
    private SpriteBatch spriteBatch;
    private Texture2D pixel;

    public LineDrawer(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
        spriteBatch = new SpriteBatch(graphicsDevice);

        // Create a 1x1 pixel texture
        pixel = new Texture2D(graphicsDevice, 1, 1);
        
    }

    public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness)
    {
        Vector2 direction = end - start;
        float length = direction.Length();
        float angle = (float)Math.Atan2(direction.Y, direction.X);
        pixel.SetData(new[] { color });
        spriteBatch.Begin();
        spriteBatch.Draw(
            pixel,
            start,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, thickness / 2), // Scale the texture to match length and thickness
            SpriteEffects.None,
            0
        );
        spriteBatch.End();
    }
}