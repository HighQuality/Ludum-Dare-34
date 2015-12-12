using Cog;
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
        const float CameraSpeed = 6f;

        public Player OurPlayer;

        public Rectangle CameraBounds = new Rectangle(new Vector2(0f, -1000f), new Vector2(4000f, 1200f));

        public MainScene()
            : base("Main Scene")
        {
            DrawAllObjects = true;

            BackgroundColor = new Color(49, 162, 242);
            CreateLocalObject<World>(Vector2.Zero);
            OurPlayer = CreateLocalObject<Player>(new Vector2(1000f, -500f));

            for (int i=0; i<20; i++)
            {
                CreateLocalObject<Cloud>(Vector2.Zero);
            }

            RegisterEvent<PhysicsUpdateEvent>(0, PhysiscUpdate);
        }

        public void PhysiscUpdate(PhysicsUpdateEvent ev)
        {
            cameraPosition += (OurPlayer.WorldCoord - cameraPosition) * Mathf.Min(1f, ev.DeltaTime * CameraSpeed);
            cameraPosition.X = Mathf.Max(CameraBounds.Left, Mathf.Min(CameraBounds.Right - Engine.Resolution.X, cameraPosition.X - Engine.Resolution.X / 2f)) + Engine.Resolution.X / 2f;
            cameraPosition.Y = Mathf.Max(CameraBounds.Top, Mathf.Min(CameraBounds.Bottom - Engine.Resolution.Y, cameraPosition.Y - Engine.Resolution.Y / 2f)) + Engine.Resolution.Y / 2f;
            Camera.WorldCoord = cameraPosition.Floor;
        }
    }
}
