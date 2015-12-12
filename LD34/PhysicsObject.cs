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
    abstract class PhysicsObject : GameObject<MainScene>
    {
        public Vector2 PositionAccumulator;
        public Vector2 Velocity;
        public float MaxSpeed = 1800f;
        public Vector2 Gravity = new Vector2(0f, 400f);
        public SpriteComponent Sprite;

        public PhysicsObject(string texture)
        {
            if (string.IsNullOrEmpty(texture) == false)
            {
                Sprite = SpriteComponent.RegisterOn(this, Resources.GetTexture(texture));
            }
            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);
        }

        public virtual void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            Velocity += Gravity * ev.DeltaTime;

            if (Velocity.Length > MaxSpeed)
            {
                Velocity.Length = MaxSpeed;
            }

            PositionAccumulator += Velocity * ev.DeltaTime;
            
            WorldCoord += new Vector2((int)PositionAccumulator.X, (int)PositionAccumulator.Y);
            PositionAccumulator -= new Vector2((int)PositionAccumulator.X, (int)PositionAccumulator.Y);
        }
    }
}
