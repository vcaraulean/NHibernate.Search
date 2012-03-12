using NHibernate.Search.Bridge;
using NHibernate.Search.Fluent.Mapping.Definitions;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Parts
{
	using Type = System.Type;

	// TODO: bridge parameters
	public class FieldBridgePart : IHasFieldBridge
	{
		private Type bridgeType;

		/// <summary>
		/// Sets a custom bridge type
		/// </summary>
		/// <typeparam name="TBridge"></typeparam>
		/// <returns></returns>
		public void Custom<TBridge>() where TBridge : IFieldBridge
		{
			Custom(typeof (TBridge));
		}

		public void Custom(Type type)
		{
			bridgeType = type;
		}

		/// <summary>
		/// Sets a Boolean Bridge
		/// </summary>
		/// <returns></returns>
		public void Boolean()
		{
			Custom(BridgeFactory.BOOLEAN.GetType());
		}

		public void DateDay()
		{
			Custom(BridgeFactory.DATE_DAY.GetType());
		}

		public void DateHour()
		{
			Custom(BridgeFactory.DATE_HOUR.GetType());
		}

		public void DateMillisecond()
		{
			Custom(BridgeFactory.DATE_MILLISECOND.GetType());
		}

		public void DateMinute()
		{
			Custom(BridgeFactory.DATE_MINUTE.GetType());
		}

		public void DateMonth()
		{
			Custom(BridgeFactory.DATE_MONTH.GetType());
		}

		public void DateSecond()
		{
			Custom(BridgeFactory.DATE_SECOND.GetType());
		}

		public void DateYear()
		{
			Custom(BridgeFactory.DATE_YEAR.GetType());
		}

		public void Double()
		{
			Custom(BridgeFactory.DOUBLE.GetType());
		}

		public void Float()
		{
			Custom(BridgeFactory.FLOAT.GetType());
		}

		public void Integer()
		{
			Custom(BridgeFactory.INTEGER.GetType());
		}

		public void Short()
		{
			Custom(BridgeFactory.SHORT.GetType());
		}

		public void String()
		{
			Custom(BridgeFactory.STRING.GetType());
		}

		public void Guid()
		{
			Custom(BridgeFactory.GUID.GetType());
		}

		public IFieldBridgeDefinition BridgeDef
		{
			get { return new FieldBridgeDefinition {Impl = bridgeType}; }
		}
	}
}