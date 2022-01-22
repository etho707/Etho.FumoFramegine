using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Etho.FumoFramegine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _crtTest;
        private Texture2D _crtTest2;
        private Rectangle _tile1;
        private RenderTarget2D _renderTarget;
        private static Rectangle _canvas = new Rectangle(0, 0, 55, 53);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferMultiSampling = false;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _crtTest = Content.Load<Texture2D>("crt_test");
            _crtTest2 = Content.Load<Texture2D>("crt_test2");

            _renderTarget = new RenderTarget2D(GraphicsDevice, _canvas.Width, _canvas.Height);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // render your complete game here at a low resolution to the render target
            // we use point sampling here because we enlarge the texture
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_crtTest, new Rectangle(0, 0, 55, 53), new Rectangle(0,0, 55, 53), Color.White);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            // here we upscale it to the full screen, again with point sampling enabled
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, 55*6, 53*6), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.AnisotropicClamp);
            var smurf = new Texture2D(GraphicsDevice, 55, 53);
            var sm = TextureTo2DArray(_crtTest2);
            var sm2 = Color2DTimes3Verticals(sm);
            var t2 = new Texture2D(GraphicsDevice, 55*3, 53*3);
            var pepe = Color2DArrayToTexture2D(sm2, t2);

            _spriteBatch.Draw(pepe, new Rectangle(55 * 6, 0, 55 * 6, 53 * 6), Color.White);
            _spriteBatch.End();

            // TODO: Add your drawing code here
            //_spriteBatch.Begin();

           // _spriteBatch.Draw(_crtTest, new Vector2 { X = 0, Y = 0 }, Color.White);

            //_spriteBatch.End();

            base.Draw(gameTime);
        }

        private Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors2D[x, y] = colors1D[x + y * texture.Width];
                }
            }
            return colors2D;
        }

        private Texture2D Color2DArrayToTexture2D(Color[,] colors2D, Texture2D target)
        {
            Color[] colors1D = new Color[target.Width * target.Height];
            for (int x = 0; x < target.Width; x++)
            {
                for (int y = 0; y < target.Height; y++)
                {
                    colors1D[x + y * target.Width] = colors2D[x, y];
                }
            }
            target.SetData(colors1D);
            return target;
        }

        private Color[,] Color2DTimes3Verticals(Color[,] colors2d)
        {
            var colors3 = new Color[colors2d.GetLength(0) * 3, colors2d.GetLength(1) * 3];
            for (int x = 0; x < colors2d.GetLength(0); x++)
                for (int y = 0; y < colors2d.GetLength(1); y++)
                {
                    /*colors3[x*3, y*3] = colors2d[x, y];
                    colors3[x * 3 + 1, y * 3] = colors2d[x, y];
                    colors3[x * 3 + 1, y * 3 + 1] = colors2d[x, y];*/
                    //colors3[x * 3 + 2, y * 3] = colors2d[x, y];

                    var myColor = Color.Black;// new Color() { A = 255, R = 10, G = 10, B = 10 };
                    //if (((double)colors2d[x, y].R + (double)colors2d[x, y].G + (double)colors2d[x, y].B) / 3 <= 255/2)
                   //     myColor = Color.Black;
                    colors3[x * 3, y * 3] = myColor;
                    colors3[x * 3 + 1, y * 3] = myColor;
                    colors3[x * 3 + 2, y * 3] = myColor;
                    colors3[x * 3, y * 3 + 1] = myColor;
                    colors3[x * 3 + 1, y * 3 + 1] = myColor;
                    colors3[x * 3 + 2, y * 3 + 1] = myColor;
                    colors3[x * 3, y * 3 + 2] = myColor;
                    colors3[x * 3 + 1, y * 3 + 2] = myColor;
                    colors3[x * 3 + 2, y * 3 + 2] = myColor;


                    //colors3[x * 3, y * 3].R = colors2d[x, y].R;
                    //colors3[x * 3 + 1, y * 3].G = colors2d[x, y].G;
                    //colors3[x * 3 + 2, y * 3].B = colors2d[x, y].B;

                    //colors3[x * 3, y * 3 + 1].R = colors2d[x, y].R;
                    //colors3[x * 3 + 1, y * 3 + 1].G = colors2d[x, y].G;
                    //colors3[x * 3 + 2, y * 3 + 1].B = colors2d[x, y].B;

                    //colors3[x * 3 + 1, y * 3 + 1] = colors2d[x, y];

                    //colors3[x * 3, y * 3 + 2].R = colors2d[x, y].R;
                    //colors3[x * 3 + 1, y * 3 + 2].G = colors2d[x, y].G;
                    //colors3[x * 3 + 2, y * 3 + 2].B = colors2d[x, y].B;

                    /*colors3[x * 3, y * 3] = RedDot(colors2d[x, y]);
                    colors3[x * 3 + 1, y * 3] = RedDot(colors2d[x, y]);
                    colors3[x * 3 + 2, y * 3] = RedDot(colors2d[x, y]);

                    colors3[x * 3, y * 3 + 1] =  GreenDot(colors2d[x, y]);
                    colors3[x * 3 + 1, y * 3 + 1] = GreenDot(colors2d[x, y]);
                    colors3[x * 3 + 2, y * 3 + 1] = GreenDot(colors2d[x, y]);

                    colors3[x * 3, y * 3 + 2] = BlueDot(colors2d[x, y]);
                    colors3[x * 3 + 1, y * 3 + 2] = BlueDot(colors2d[x, y]);
                    colors3[x * 3 + 2, y * 3 + 2] = BlueDot(colors2d[x, y]);*/

                    colors3[x * 3, y * 3] = RedDot(colors2d[x, y]);
                    colors3[x * 3 + 1, y * 3] = RedDot(colors2d[x, y]);

                    colors3[x * 3, y * 3 + 1] = GreenDot(colors2d[x, y]);
                    colors3[x * 3 + 1, y * 3 + 1] = GreenDot(colors2d[x, y]);

                    colors3[x * 3 + 2, y * 3] = BlueDot(colors2d[x, y]);
                    colors3[x * 3 + 2, y * 3 + 1] = BlueDot(colors2d[x, y]);
                }
            return colors3;
        }

        private static byte Dim(byte b)
        {
            var half = (int)b * (0.7);
            return (byte)half;
        }

        private Color RedDot(Color color)
        {
            color.B = Dim(color.B);
            color.G = Dim(color.G);
            return color;
        }

        private Color GreenDot(Color color)
        {
            color.B = Dim(color.B);
            color.R = Dim(color.R);
            return color;
        }

        private Color BlueDot(Color color)
        {
            color.G = Dim(color.G);
            color.R = Dim(color.R);
            return color;
        }
    }
}