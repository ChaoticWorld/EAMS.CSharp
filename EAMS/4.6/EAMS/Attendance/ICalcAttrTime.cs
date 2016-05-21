using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAttendanceDevice;
using Attendance.Model;

namespace Attendance
{
    public interface ICalcAttrTime
    {
        void calcAttrTime(RecordModel record, List<ICardTime> cardTimes, ClassPlanModel cpModel, List<FeeCalculatorModel> feeCalcs);
    }
    public abstract class AttrTime : ICalcAttrTime
    {
        protected FeeCalculatorModel getFeeCalc(int Offset,List<FeeCalculatorModel> feeCalcs)
        {
            FeeCalculatorModel feeCalc = null;
            if (Offset < 0)//-240分钟为4小时即半天
                feeCalc = (Offset < -240) ? feeCalcs.First(f => f.dateEnum == "Leave") : feeCalcs.First(f => f.dateEnum == "Absence");
            else feeCalc = feeCalcs.First(f => f.dateEnum == "Over");
            return feeCalc;
        }
        public abstract void calcAttrTime(RecordModel record, List<ICardTime> cardTimes, ClassPlanModel cpModel, List<FeeCalculatorModel> feeCalcs);
    }
    public class HolidayAttrTime : AttrTime
    {
        public override void calcAttrTime(RecordModel record, List<ICardTime> cardTimes, ClassPlanModel cpModel, List<FeeCalculatorModel> feeCalcs)
        {
            DateTime cardMin, cardMax;

            if (cardTimes != null && cardTimes.Count > 0)
            {
                cardMin = cardTimes.Min(min => min.cardTime);
                cardMax = cardTimes.Max(max => max.cardTime);
                record.bAttTimeStr = cardMin.ToShortTimeString();
                record.eAttTimeStr = cardMax.ToShortTimeString();
                record.bOffset = int.MaxValue;
                record.eOffset = AttendanceBLL.difTime(cardMin, cardMax);
                record.bOffsetFee = 0;
                var feeCalc = feeCalcs.First(f=>f.dateEnum == "Holiday");
                record.eOffsetFee = Math.Ceiling((decimal)record.eOffset.Value / feeCalc.Unit.Value) * feeCalc.UnitFee;
                record.eOffsetFee = Math.Min(record.eOffsetFee.Value, feeCalc.MaxFee.Value);
            }
            else {
                record.bAttTimeStr = "0:00";
                record.eAttTimeStr = "0:00";
                record.bOffset = int.MaxValue;//Min-周末:Max-节日
                record.eOffset = 0;
                record.bOffsetFee = 0;
                record.eOffsetFee = 0;
            }
        }
    }
    public class DayOffAttrTime : AttrTime
    {
        public override void calcAttrTime(RecordModel record, List<ICardTime> cardTimes, ClassPlanModel cpModel, List<FeeCalculatorModel> feeCalcs)
        {
            DateTime cardMin, cardMax;

            if (cardTimes != null && cardTimes.Count > 0)
            {
                cardMin = cardTimes.Min(min => min.cardTime);
                cardMax = cardTimes.Max(max => max.cardTime);
                record.bAttTimeStr = cardMin.ToShortTimeString();
                record.eAttTimeStr = cardMax.ToShortTimeString();
                record.bOffset = int.MinValue;
                record.eOffset = AttendanceBLL.difTime(cardMin, cardMax);
                record.bOffsetFee = 0;
                var feeCalc = feeCalcs.First(f => f.dateEnum == "DayOff");
                record.eOffsetFee = Math.Ceiling((decimal)record.eOffset.Value / feeCalc.Unit.Value) * feeCalc.UnitFee;
                record.eOffsetFee = Math.Min(record.eOffsetFee.Value, feeCalc.MaxFee.Value);
            }
            else
            {
                record.bAttTimeStr = "0:00";
                record.eAttTimeStr = "0:00";
                record.bOffset = int.MinValue;//Min-周末:Max-节日
                record.eOffset = 0;
                record.bOffsetFee = 0;
                record.eOffsetFee = 0;
            }
        }
    }

    public class WorkDayAttrTime : AttrTime
    {
        public override void calcAttrTime(RecordModel record, List<ICardTime> cardTimes, ClassPlanModel cpModel, List<FeeCalculatorModel> feeCalcs)
        {
            DateTime cardMin, cardMax;
            FeeCalculatorModel feeCalc = null;
            if (cardTimes != null && cardTimes.Count > 0)
            {
                cardMin = cardTimes.Min(min => min.cardTime);
                cardMax = cardTimes.Max(max => max.cardTime);
                record.bAttTimeStr = cardMin.ToShortTimeString();
                record.eAttTimeStr = cardMax.ToShortTimeString();
                record.bOffset = AttendanceBLL.difTime(cardMin, DateTime.Parse(record.sDate.Value.ToString("yyyy-MM-dd ")+cpModel.bTime));
                record.eOffset = AttendanceBLL.difTime(DateTime.Parse(record.sDate.Value.ToString("yyyy-MM-dd ") + cpModel.eTime), cardMax);

                feeCalc =  getFeeCalc(record.bOffset.Value, feeCalcs);
                record.bOffsetFee = Math.Ceiling((decimal)record.bOffset.Value / feeCalc.Unit.Value) * feeCalc.UnitFee;
                record.bOffsetFee = (feeCalc.MaxFee < 0) ?
                    Math.Max(record.bOffsetFee.Value, feeCalc.MaxFee.Value) :
                    Math.Min(record.bOffsetFee.Value, feeCalc.MaxFee.Value);

                feeCalc = getFeeCalc(record.eOffset.Value, feeCalcs);
                record.eOffsetFee = Math.Ceiling((decimal)record.eOffset.Value / feeCalc.Unit.Value) * feeCalc.UnitFee;
                record.eOffsetFee = (feeCalc.MaxFee < 0) ?
                    Math.Max(record.eOffsetFee.Value, feeCalc.MaxFee.Value) :
                    Math.Min(record.eOffsetFee.Value, feeCalc.MaxFee.Value);
            }
            else
            {
                record.bAttTimeStr = "0:00";
                record.eAttTimeStr = "0:00";
                record.bOffset = -9999;
                record.eOffset = -9999;
                feeCalc = getFeeCalc(record.bOffset.Value, feeCalcs);
                record.bOffsetFee = feeCalc.MaxFee;
                record.eOffsetFee = feeCalc.MaxFee;
            }
        }
    }

    //值班处理
    public class OnDutyAttrTime : AttrTime
    {
        public override void calcAttrTime(RecordModel record, List<ICardTime> cardTimes, ClassPlanModel cpModel, List<FeeCalculatorModel> feeCalcs)
        { }
    }
}
