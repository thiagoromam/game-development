using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Interop.Module;

namespace MonogameWpf2.GameModules
{
    // https://github.com/alaskajohn/2dGPfG/tree/master/2dGPfG-XNA/9-1-2-PixelModBlur-GPU
    public class BlurGameModule : BaseGameModule
    {
        Texture2D pixelsTexture1, pixelsTexture2, spriteSheet;
        Color backgroundColor = Color.Black;
        FrameRateCounter g_frameRate = new FrameRateCounter();
        Effect blurEffect;
        int type = 4;
        //if 0, create gradient
        //if 1, invert snowman
        //if 2, invert snowman in radius
        //if 3, CPU based blur
        //if 4, GPU based blur
        Vector2 pos = Vector2.Zero;
        Vector2 vel = new Vector2(1.0f, 1.5f);

        public BlurGameModule()
            : base("Content")
        {
        }

        public override void Initialize()
        {
            Width = 1280;
            Height = 720;
        }
        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            g_frameRate.LoadContent(Content);
            spriteSheet = Content.Load<Texture2D>("Images/snow_assets");

            blurEffect = Content.Load<Effect>("Effects/blur");

            if (type == 0)
                createGradient();
            if (type == 1)
                invertSnowman();
            if (type == 2)
                invertSnowman();
        }
        public override void Update(GameTime gameTime)
        {
            g_frameRate.Update(gameTime);

            if (type == 2)
                modifyTexture2(gameTime);
        }
        public override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (type == 0)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(pixelsTexture1, Vector2.Zero, Color.White);
                SpriteBatch.End();
            }
            if ((type == 1) || (type == 2))
            {
                SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                SpriteBatch.Draw(pixelsTexture1, Vector2.Zero, Color.White);
                SpriteBatch.Draw(pixelsTexture2, new Vector2(256, 0), Color.White);
                SpriteBatch.End();
            }
            if ((type == 3))
            {
                int scr_width = 1280;
                int scr_height = 720;

                RenderTargetBinding[] tempBinding = GraphicsDevice.GetRenderTargets();

                RenderTarget2D tempRenderTarget = new RenderTarget2D(GraphicsDevice, scr_width, scr_height);
                GraphicsDevice.SetRenderTarget(tempRenderTarget);

                // Render a simple scene.
                GraphicsDevice.Clear(Color.White);
                SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 3; j++)
                        SpriteBatch.Draw(spriteSheet, new Vector2(i * 256, j * 256), new Rectangle(0, 128, 256, 256), Color.White);
                SpriteBatch.End();

                GraphicsDevice.SetRenderTargets(tempBinding);

                //Get 2D array of colors from sprite sheet

                Color[] arrayOfColor = new Color[scr_width * scr_height];
                tempRenderTarget.GetData<Color>(arrayOfColor);


                Vector2 center = new Vector2(scr_width / 2.0f, scr_height / 2.0f);
                double maxDistSQR = Math.Sqrt(Math.Pow(center.X, 2) + Math.Pow(center.Y, 2));

                for (int j = 0; j < scr_height; j++)
                    for (int i = 0; i < scr_width; i++)
                    {
                        double distSQR = Math.Sqrt(Math.Pow(i - center.X, 2) + Math.Pow(j - center.Y, 2));
                        int blurAmount = (int)Math.Floor(10 * distSQR / maxDistSQR);

                        int currElement = i + (scr_width * j);
                        int prevElement = currElement - blurAmount;
                        int nextElement = currElement + blurAmount;
                        if (((currElement - blurAmount) > 0) && ((currElement + blurAmount) < (scr_width * scr_height)))
                        {
                            arrayOfColor[currElement].R = (byte)((arrayOfColor[currElement].R + arrayOfColor[prevElement].R + arrayOfColor[nextElement].R) / 3.0f);
                            arrayOfColor[currElement].G = (byte)((arrayOfColor[currElement].G + arrayOfColor[prevElement].G + arrayOfColor[nextElement].G) / 3.0f);
                            arrayOfColor[currElement].B = (byte)((arrayOfColor[currElement].B + arrayOfColor[prevElement].B + arrayOfColor[nextElement].B) / 3.0f);
                        }
                    }

                //Place color array into a texture
                Texture2D newTexture = new Texture2D(GraphicsDevice, scr_width, scr_height);
                newTexture.SetData<Color>(arrayOfColor);

                SpriteBatch.Begin();
                SpriteBatch.Draw(newTexture, Vector2.Zero, Color.White);
                SpriteBatch.End();
            }
            if (type == 4)
            {
                int scr_width = 1280;
                int scr_height = 720;

                RenderTargetBinding[] tempBinding = GraphicsDevice.GetRenderTargets();

                RenderTarget2D tempRenderTarget = new RenderTarget2D(GraphicsDevice, scr_width, scr_height);
                GraphicsDevice.SetRenderTarget(tempRenderTarget);

                // Render a simple scene.
                GraphicsDevice.Clear(Color.White);
                SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 3; j++)
                        SpriteBatch.Draw(spriteSheet, new Vector2(i * 256, j * 256), new Rectangle(0, 128, 256, 256), Color.White);
                SpriteBatch.End();

                GraphicsDevice.SetRenderTargets(tempBinding);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                blurEffect.CurrentTechnique.Passes[0].Apply();
                SpriteBatch.Draw(tempRenderTarget, Vector2.Zero, Color.White);
                SpriteBatch.End();
            }

            g_frameRate.Draw(SpriteBatch);
        }

        public void createGradient()
        {

            int width = 256;
            int height = 256;


            //Create 2D array of colors
            Color[] arrayOfColor = new Color[width * height];

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    arrayOfColor[j + (width * i)] = new Color(i, 0, j);
                }

            //Place color array into a texture
            pixelsTexture1 = new Texture2D(GraphicsDevice, width, height);
            pixelsTexture1.SetData<Color>(arrayOfColor);
        }
        public void invertSnowman()
        {

            int width = 256;
            int height = 256;


            //Get 2D array of colors from sprite sheet
            Color[] arrayOfColor = new Color[width * height];
            spriteSheet.GetData<Color>(0, new Rectangle(0, 128, 256, 256), arrayOfColor, 0, (width * height));

            //Place color array into a texture
            pixelsTexture1 = new Texture2D(GraphicsDevice, width, height);
            pixelsTexture1.SetData<Color>(arrayOfColor);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    int currentElement = j + (width * i);
                    arrayOfColor[currentElement].R = (byte)(255 - arrayOfColor[currentElement].R);
                    arrayOfColor[currentElement].G = (byte)(255 - arrayOfColor[currentElement].G);
                    arrayOfColor[currentElement].B = (byte)(255 - arrayOfColor[currentElement].B);
                }

            //Place color array into a texture
            pixelsTexture2 = new Texture2D(GraphicsDevice, width, height);
            pixelsTexture2.SetData<Color>(arrayOfColor);
        }
        public void updatePosition()
        {
            pos += vel;

            if ((pos.X < 0) || (pos.X > 255))
                vel.X *= -1f;
            MathHelper.Clamp(pos.X, 0, 255);

            if ((pos.Y < 0) || (pos.Y > 255))
                vel.Y *= -1f;
            MathHelper.Clamp(pos.Y, 0, 255);
        }
        public void modifyTexture2(GameTime gameTime)
        {
            updatePosition();


            int width = 256;
            int height = 256;

            //Get 2D array of colors from texture2
            Color[] arrayOfColor = new Color[width * height];
            pixelsTexture1.GetData<Color>(arrayOfColor);

            //Modify color array into a texture
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    int currentElement = j + (width * i);
                    double distance = Math.Sqrt(Math.Pow(i - pos.X, 2) + Math.Pow(j - pos.Y, 2));
                    double radius = 50;// (Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 500.0f) * 50) + 50;
                    if (distance < radius)
                    {
                        arrayOfColor[currentElement].R = (byte)(255 - arrayOfColor[currentElement].R);
                        arrayOfColor[currentElement].G = (byte)(255 - arrayOfColor[currentElement].G);
                        arrayOfColor[currentElement].B = (byte)(255 - arrayOfColor[currentElement].B);
                        arrayOfColor[currentElement].A = 255;
                    }
                }


            //Place color array into a texture
            pixelsTexture2.SetData<Color>(arrayOfColor);
        }
    }
}