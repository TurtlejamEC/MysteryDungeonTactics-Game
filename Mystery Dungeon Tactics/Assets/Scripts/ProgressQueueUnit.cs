using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ProgressQueueUnit {
	public int CharacterId { get; set; }
	public int Progress { get; set; }

	public ProgressQueueUnit(int characterId, int progress) {
		CharacterId = characterId;
		Progress = progress;
	}
}