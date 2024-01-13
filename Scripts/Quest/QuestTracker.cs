using System;

namespace IdleCore
{
    public class QuestTracker
    {
        public readonly QuestType Type;
        private Action _questResponse;
        private Action<int> _questNumberResponse;

        public QuestTracker(QuestType type)
        {
            Type = type;
        }

        public void Init(Action response)
        {
            _questResponse = response;
        }
        
        public void Init(Action<int> response)
        {
            _questNumberResponse = response;
        }

        public void ResponseQuest(int responses = 1)
        {
            if (responses > 1)
                _questNumberResponse?.Invoke(responses);
            else
                _questResponse?.Invoke();
        }

        public void Completed()
        {
            _questResponse = null;
            _questNumberResponse = null;
        }
    }
}