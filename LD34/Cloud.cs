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
            Depth = 5f; // + Engine.RandomFloat() * 20f;
            
            Texture texture = Resources.GetTexture("cloud" + (1 + (int)(Engine.RandomFloat() * 3f)).ToString());
            SpriteComponent.RegisterOn(this, texture);
            Size = texture.Size;
        }

        public void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            if (WorldCoord.X - Scene.Camera.WorldCoord.X + Engine.Resolution.X / 2f < -300f)
            {
                Remove();
            }
        }
    }
}
