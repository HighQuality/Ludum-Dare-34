using Cog;
using Cog.Modules.Content;
using Cog.Modules.Renderer;
using Cog.Modules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD34
{
    [Resource(ContainerName = "main", Filename = "tileset.png", Key = "tileset")]
    class World : GameObject<MainScene>
    {
        public Texture Texture;

        public World()
        {
            Texture = Resources.GetTexture("tileset");
            RegisterEvent<DrawEvent>(0, Draw);
        }

        public void Draw(DrawEvent ev)
        {
            int start = (int)((Scene.Camera.WorldCoord.X - Engine.Resolution.X / 2f) / 64f) - 1;
            int width = (int)(Engine.Resolution.X / 64f) + 2;
            for (int x = start; x < start + width; x++)
            {
                ev.RenderTarget.DrawTexture(Texture, new Vector2(x * 64f, 0f).Floor, Color.White, Vector2.One, Vector2.Zero, 0f, new Rectangle(Vector2.Zero, new Vector2(64f, 64f)));
                for (int y=0; y<10; y++)
                {
                    ev.RenderTarget.DrawTexture(Texture, new Vector2(x * 64f, 64f + y * 64f).Floor, Color.White, Vector2.One, Vector2.Zero, 0f, new Rectangle(new Vector2(64f, 0f), new Vector2(64f, 64f)));
                }
            }
        }
    }
}
