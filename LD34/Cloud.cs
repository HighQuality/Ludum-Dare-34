using Cog;
using Cog.Modules.Content;
using Cog.Modules.EventHost;
using Cog.Modules.Renderer;
using Cog.Modules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD34
{
    [Resource(ContainerName = "main", Filename = "cloud.png", Key = "cloud1")]
    [Resource(ContainerName = "main", Filename = "cloud_2.png", Key = "cloud2")]
    [Resource(ContainerName = "main", Filename = "cloud_3.png", Key = "cloud3")]
    [Resource(ContainerName = "main", Filename = "cloud_4.png", Key = "cloud4")]
    class Cloud : GameObject<MainScene>
    {
        public Cloud()
        {
            Depth = 5f + Engine.RandomFloat() * 20f;

            do
            {
                LocalCoord = Scene.CameraBounds.TopLeft + new Vector2(4000f, 800f) * new Vector2(Engine.RandomFloat(), Engine.RandomFloat());
            } while (Scene.EnumerateObjects<Cloud>().Select(o => o == this ? 9999f : (o.LocalCoord - LocalCoord).Length).Min() < 300f);

            Texture texture = Resources.GetTexture("cloud" + (1 + (int)(Engine.RandomFloat() * 3f)).ToString());
            SpriteComponent.RegisterOn(this, texture);
            Size = texture.Size;

            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);
        }

        public void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            LocalCoord += new Vector2(Depth * ev.DeltaTime, 0f);
            if (LocalCoord.X >= Scene.CameraBounds.Right)
            {
                LocalCoord = new Vector2(0f, LocalCoord.Y);
            }
        }
    }
}
