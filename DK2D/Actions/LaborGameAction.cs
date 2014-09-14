namespace DK2D.Actions
{
    abstract class LaborGameAction : GameAction
    {
        public float Duration { get; set; }

        public float PercentCompleted { get; private set; }

        public override void Perform(float secondsElapsed)
        {
            PercentCompleted += (100 / Duration) * secondsElapsed;

            if (PercentCompleted >= 100)
            {
                IsDone = true;

                Completed();
            }
        }

        protected abstract void Completed();
    }
}
