using System;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace World
{
    public class HeroController : MonoBehaviour
    {
        private WorldGraphController _worldGraphController;
        public GameObject UnitVisualizer;
      
        private void Start()
        {
            _worldGraphController = GetComponent<WorldGraphController>();
        }

        public GameObject SpawnHeros(List<Unit> heroes, WorldGraphController graph)
        {
            var heroContainer = new GameObject("Hero Container");
            heroContainer.transform.parent = transform;
            heroContainer.transform.position = graph.TavernNodeController.gameObject.transform.position;


            foreach (var hero in heroes)
            {
                var unitObject = Instantiate(UnitVisualizer);
                unitObject.transform.parent = heroContainer.transform;
                unitObject.transform.localPosition = Vector3.zero;
                unitObject.GetComponent<UnitVisualizer>().Unit = hero;
            }

            var mover = heroContainer.AddComponent<TestNodeMover>();
            mover.graph = graph;
            mover.currentNode = graph.TavernNodeController;

            return heroContainer;
        }
    }
}