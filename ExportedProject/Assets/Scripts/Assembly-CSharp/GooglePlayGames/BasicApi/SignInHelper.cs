using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.BasicApi
{
	public class SignInHelper
	{
		private static int True;

		private static int False = 1;

		private const string PromptSignInKey = "prompt_sign_in";

		public static SignInStatus ToSignInStatus(int code)
		{
			Dictionary<int, SignInStatus> dictionary = new Dictionary<int, SignInStatus>();
			dictionary.Add(-12, SignInStatus.AlreadyInProgress);
			dictionary.Add(0, SignInStatus.Success);
			dictionary.Add(4, SignInStatus.UiSignInRequired);
			dictionary.Add(7, SignInStatus.NetworkError);
			dictionary.Add(8, SignInStatus.InternalError);
			dictionary.Add(10, SignInStatus.DeveloperError);
			dictionary.Add(16, SignInStatus.Canceled);
			dictionary.Add(17, SignInStatus.Failed);
			dictionary.Add(12500, SignInStatus.Failed);
			dictionary.Add(12501, SignInStatus.Canceled);
			dictionary.Add(12502, SignInStatus.AlreadyInProgress);
			Dictionary<int, SignInStatus> dictionary2 = dictionary;
			return (!dictionary2.ContainsKey(code)) ? SignInStatus.Failed : dictionary2[code];
		}

		public static void SetPromptUiSignIn(bool value)
		{
			PlayerPrefs.SetInt("prompt_sign_in", (!value) ? False : True);
		}

		public static bool ShouldPromptUiSignIn()
		{
			return PlayerPrefs.GetInt("prompt_sign_in", True) != False;
		}
	}
}
