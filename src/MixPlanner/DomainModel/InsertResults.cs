using System;

namespace MixPlanner.DomainModel
{
    public class InsertResults
    {
        InsertResults(
            bool isSuccess, 
            IMixingStrategy startStrategy, 
            int insertIndex, 
            IMixingStrategy endStrategy)
        {
            InsertIndex = insertIndex;
            EndStrategy = endStrategy;
            IsSuccess = isSuccess;
            StartStrategy = startStrategy;
        }

        public static InsertResults SuccessEmptyMix()
        {
            return Success(null, 0, null);
        }

        public static InsertResults Success(
            IMixingStrategy startStrategy, 
            int insertIndex,
            IMixingStrategy endStrategy)
        {
            if (insertIndex > 0 && startStrategy == null)
                throw new ArgumentNullException("startStrategy", "Start strategy can only be null if inserting at the start of the mix (i.e. insert index = 0).");
            if (insertIndex < 0)
                throw new ArgumentOutOfRangeException("insertIndex", insertIndex, "Insert index cannot be negative.");

            return new InsertResults(true, startStrategy, insertIndex, endStrategy);
        }

        public static InsertResults Failure()
        {
            return new InsertResults(false, null, -1, null);
        }

        public bool IsSuccess { get; private set; }
        public IMixingStrategy StartStrategy { get; set; }
        public int InsertIndex { get; private set; }
        public IMixingStrategy EndStrategy { get; set; }
    }
}