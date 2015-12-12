using Cog;
using Cog.Modules.Content;
using Cog.Modules.EventHost;
using Cog.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD34
{
    class MainScene : Scene
    {
        Vector2 cameraPosition;
        float cloudSpawnX;

        public MainScene()
            : base("Main Scene")
        {
            DrawAllObjects = true;
            cameraPosition = new Vector2(0f, -200f);
            Camera.WorldCoord = cameraPosition;

            BackgroundColor = new Color(49, 162, 242);
            CreateLocalObject<World>(Vector2.Zero);

            cloudSpawnX = -Engine.Resolution.X / 2f;

            RegisterEvent<PhysicsUpdateEvent>(0, PhysiscUpdate);
        }

        public void PhysiscUpdate(PhysicsUpdateEvent ev)
        {
            while (cloudSpawnX < Camera.WorldCoord.X + Engine.Resolution.X / 2f + 300f)
            {
                CreateLocalObject<Cloud>(new Vector2(cloudSpawnX, -500f + Engine.RandomFloat() * 200f));
                cloudSpawnX += 300f + Engine.RandomFloat() * 200f;
            }

            cameraPosition += new Vector2(600f * ev.DeltaTime, 0f);
            Camera.WorldCoord = cameraPosition.Floor;
        }
    }
}
