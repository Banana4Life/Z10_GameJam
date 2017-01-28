using System.Collections.Generic;
using UnityEngine;
using World;

namespace Model
{
    [RequireComponent(typeof(WorldGraphController))]
    public class WorldLoop : MonoBehaviour
    {
        private WorldGraphController _graphController;
        public GameObject ObjectiveControllerPrefab;
        public GameObject UnitVisualizerPrefab;
        private ObjectiveGenerator _objectiveGenerator = new ObjectiveGenerator();

        void Start()
        {
            _graphController = GetComponent<WorldGraphController>();
        }

        void Update()
        {
            GenerateObjectives();
            GenerateMonsters();
            WaitForObjectiveSelection();
            WaitForPartySelection();

            var party = SpawnParty();

            while (party.IsAlive)
            {
                MoveParty(party);
                MoveMonsters();

                TriggerEvents();

                if (party.ReachedTarget)
                {
                    party.SetTarget(TavernNode);
                }
            }
        }

        private void GenerateMonsters()
        {
            var monsterNodes = new HashSet<NodeController>();
            for (int i = 0; i < Mathf.CeilToInt((Random.value + 0.3f) * GameData.MaxMonsters); i++)
            {
                var node = _graphController.Nodes.Random();
                monsterNodes.Add(node);
            }

            foreach (var node in monsterNodes)
            {
                var gameObject = CreateMonsterParty();
                
            }
        }

        private GameObject CreateMonsterParty()
        {
            var unitObject = Instantiate(UnitVisualizerPrefab);
            var unit = _unitGenerator.GenerateUnit();
            unitObject.GetComponent<UnitVisualizer>().Unit = unit;
        }

        private void GenerateObjectives()
        {
            var objectiveNodes = new HashSet<NodeController>();
            for (int i = 0; i < Mathf.CeilToInt(Random.value*GameData.MaxTreasures); i++)
            {
                var node = _graphController.Nodes.Random();
                objectiveNodes.Add(node);
            }

            foreach (var node in objectiveNodes)
            {
                var gameObject = Instantiate(ObjectiveControllerPrefab);
                var objectiveController = gameObject.GetComponent<ObjectiveController>();
                objectiveController.NodeController = node;
                objectiveController.Objective = _objectiveGenerator.GenerateObjective(_graphController.TavernDistance(node));
            }


        }

        private static void MoveParty(object party)
        {
            if (party.KnowsPathToTarget)
            {
                party.MoveTowardsTarget();
            }
            else
            {
                party.MoveRandom();
            }
        }
    }
}