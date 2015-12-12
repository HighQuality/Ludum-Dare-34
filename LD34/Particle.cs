using Cog;
using Cog.Modules.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD34
{
    class Particle
    {
        public Texture Texture;
        public Vector2 Position;
        public Vector2 BeginSpeed,
            EndSpeed;
        public Vector2 BeginScale,
            EndScale;
        public float BeginRotation,
            EndRotation;
        public Color BeginColor,
            EndColor;
        public float Age,
            DiesAfter;

        public static Queue<Particle> Queue = new Queue<Particle>();

        public static Particle Create(Texture texture, Vector2 position, Vector2 beginSpeed, Vector2 endSpeed, Vector2 beginScale, Vector2 endScale, float beginRotation, float endRotation, Color beginColor, Color endColor, float diesAfter)
        {
            Particle p = null;
            if (Queue.Count == 0)
            {
                p = new Particle();
            }
            else
            {
                p = Queue.Dequeue();
            }
            p.Texture = texture;
            p.Position = position;
            p.BeginSpeed = beginSpeed;
            p.EndSpeed = endSpeed;
            p.BeginScale = beginScale;
            p.EndScale = endScale;
            p.BeginRotation = beginRotation;
            p.EndRotation = endRotation;
            p.BeginColor = beginColor;
            p.EndColor = endColor;
            p.Age = 0f;
            p.DiesAfter = diesAfter;
            return p;
        }

        public static void Recycle(Particle particle)
        {
            Queue.Enqueue(particle);
        }
        
        private Particle()
        {
        }

        public bool UpdateAndDraw(float deltaTime, IRenderTarget renderTarget)
        {
            Age += deltaTime;
            if (Age < DiesAfter)
            {
                float progress = Age / DiesAfter;
                Position += (BeginSpeed + (EndSpeed - BeginSpeed) * progress) * deltaTime;
                if (Position.Y >= 0f)
                {
                    Position.Y = 0f;
                    BeginSpeed.Y *= -1f;
                    EndSpeed.Y *= -1f;
                }

                renderTarget.DrawTexture(Texture, Position, BeginColor.Transition(EndColor, progress), BeginScale + (EndScale - BeginScale) * progress, Texture.Size / 2f, BeginRotation + (EndRotation - BeginRotation) * progress, new Rectangle(Vector2.Zero, Texture.Size));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
