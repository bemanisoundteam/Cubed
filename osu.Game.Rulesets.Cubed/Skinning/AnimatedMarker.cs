using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public sealed partial class AnimatedMarker : CompositeDrawable, IMarker {
        private readonly int perfectAt;

        private readonly Texture[] ApproachTextures;
        private readonly Texture[] PerfectTextures;
        private readonly Texture[] GreatTextures;
        private readonly Texture[] GoodTextures;
        private readonly Texture[] MehTextures;

        private Texture[] judgementTextures;
        private double judgementStartTime;
        private double judgementDuration;
        private double approachStartTime;
        private double approachDuration;

        public AnimatedMarker(TextureStore textures, int approachCount, int tapAtFrame, int judgementCount) {
            perfectAt = tapAtFrame;

            ApproachTextures = new Texture[approachCount];
            PerfectTextures = new Texture[judgementCount];
            GreatTextures = new Texture[judgementCount];
            GoodTextures = new Texture[judgementCount];
            MehTextures = new Texture[judgementCount];

            for (int i = 0; i < approachCount; i++)
                ApproachTextures[i] = textures.Get($"Approach{i}.png");
            for (int i = 0; i < judgementCount; i++)
                PerfectTextures[i] = textures.Get($"Perfect{i}.png");
            for (int i = 0; i < judgementCount; i++)
                GreatTextures[i] = textures.Get($"Great{i}.png");
            for (int i = 0; i < judgementCount; i++)
                GoodTextures[i] = textures.Get($"Good{i}.png");
            for (int i = 0; i < judgementCount; i++)
                MehTextures[i] = textures.Get($"Meh{i}.png");

            AddInternal(TextureHolder);
        }

        public void AnimateApproach(double time) {
            approachStartTime = TransformStartTime;
            approachDuration = time * ApproachTextures.Length / perfectAt;

            judgementStartTime = 0;
        }

        public void AnimateHit(double duration, JudgementResult judgement) {
            judgementTextures = judgement.Type switch {
                HitResult.Perfect => PerfectTextures,
                HitResult.Great => GreatTextures,
                HitResult.Good => GoodTextures,
                HitResult.Meh => MehTextures,
                _ => throw new System.ArgumentOutOfRangeException(nameof(judgement))
            };

            judgementStartTime = TransformStartTime;
            judgementDuration = duration;
        }

        protected override void Update() {
            bool isJudgement = judgementStartTime != 0 && Time.Current >= judgementStartTime;

            double startTime = isJudgement ? judgementStartTime : approachStartTime;
            double duration = isJudgement ? judgementDuration : approachDuration;
            Texture[] textures = isJudgement ? judgementTextures : ApproachTextures;

            if (Time.Current < startTime || Time.Current >= startTime + duration)
                SetTexture(null);
            else {
                int currentFrame = (int) ((Time.Current - startTime) / duration * textures.Length);
                SetTexture(textures[currentFrame]);
            }
        }

        private readonly Sprite TextureHolder = new Sprite {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        };

        private void SetTexture(Texture texture) => TextureHolder.Texture = texture;
    }
}
