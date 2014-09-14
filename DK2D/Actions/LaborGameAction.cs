namespace DK2D.Actions
{
    abstract class LaborGameAction : GameAction
    {
        public float Duration { get; set; }

        public float PercentCompleted { get; private set; }

        public override void Perform(float secondsElapsed, Objects.Creatures.Imp imp)
        {
            PercentCompleted += (100 / Duration) * secondsElapsed;

            if (PercentCompleted >= 100)
            {
                IsDone = true;

                Completed(imp);
            }
        }

        protected abstract void Completed(Objects.Creatures.Imp imp);
    }
}
