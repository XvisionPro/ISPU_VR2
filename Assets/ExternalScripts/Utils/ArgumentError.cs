using System;

namespace ExternalScripts
{
	public class ArgumentError: Exception
	{
		public ArgumentError(): base("ArgumentError")
		{

		}
	}
}