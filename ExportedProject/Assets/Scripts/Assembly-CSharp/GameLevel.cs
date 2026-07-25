using System;

[Serializable]
public class GameLevel
{
	private int level;

	private int xp;

	private int[] xpToLevelUp = new int[3] { 4, 20, 36 };

	public bool ignoreXp;

	public int GetLevel()
	{
		if (ignoreXp)
		{
			return 0;
		}
		return level;
	}

	public int GetActualLevel()
	{
		return level;
	}

	public void SetLevel(int l)
	{
		if (l >= 0 && l <= 3)
		{
			level = l;
		}
	}

	public int GetXp()
	{
		return xp;
	}

	public void SetXp(int n)
	{
		xp = n;
		CalculateLevel();
	}

	public void AddXp()
	{
		if (!ignoreXp)
		{
			xp++;
			CalculateLevel();
		}
	}

	public void ReduceXp()
	{
		if (!ignoreXp)
		{
			xp -= 4;
			CalculateLevel();
		}
	}

	public void IgnoreXp(bool b)
	{
		if (b)
		{
			ignoreXp = true;
		}
		else
		{
			ignoreXp = false;
		}
	}

	public void CalculateLevel()
	{
		if (GlobalControl.globalControl.mode == "survival")
		{
			xpToLevelUp = new int[3] { 2, 10, 18 };
		}
		else
		{
			xpToLevelUp = new int[3] { 4, 20, 36 };
		}
		switch (level)
		{
		case 0:
			if (xp > xpToLevelUp[level])
			{
				level++;
				xp = 4;
			}
			else if (xp < 0)
			{
				xp = 0;
			}
			break;
		case 1:
			if (xp > xpToLevelUp[level])
			{
				level++;
				xp = 4;
			}
			else if (xp < 0)
			{
				level--;
				xp = 0;
			}
			break;
		case 2:
			if (xp > xpToLevelUp[level])
			{
				level++;
				xp = 4;
			}
			else if (xp < 0)
			{
				level--;
				xp = xpToLevelUp[level] / 2;
			}
			break;
		case 3:
			if (xp > 4)
			{
				xp = 4;
			}
			else if (xp < 0)
			{
				level--;
				xp = xpToLevelUp[level] / 2;
			}
			break;
		}
	}
}
