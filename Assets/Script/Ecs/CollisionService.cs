using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Script.Ecs
{
    public sealed class CollisionService : ICollisionService
    {
        private struct CollidedBody
        {
            public EcsPackedEntity Entity;
            public Vector3 Position;
            public float Radius;
        }

        private readonly struct CellCoordinate : IEqualityComparer<CellCoordinate>
        {
            public readonly int X;
            public readonly int Y;

            public CellCoordinate(int x, int y)
            {
                X = x;
                Y = y;
            }

            public bool Equals(CellCoordinate x, CellCoordinate y)
            {
                return x.X == y.X && x.Y == y.Y;
            }

            public int GetHashCode(CellCoordinate obj)
            {
                return HashCode.Combine(obj.X, obj.Y);
            }
        }

        private sealed class Cell
        {
            public readonly List<CollidedBody> Bodies = new ();
        }

        private readonly List<CollidedBody> _bodies = new ();
        private readonly float _cellSize;
        private readonly Dictionary<CellCoordinate, Cell> _grid = new ();

        public CollisionService(float cellSize)
        {
            _cellSize = cellSize;
        }
        
        public bool TryFindCollidedEntity(Vector3 position, out EcsPackedEntity entity)
        {
            if (_grid.TryGetValue(ToCellCoordinate(position), out var cell))
            {
                var bodyIndex = cell.Bodies.FindIndex(b => Vector3.Distance(b.Position, position) < b.Radius);
                if (bodyIndex >= 0)
                {
                    entity = cell.Bodies[bodyIndex].Entity; 
                    return true;
                }
            }
            
            entity = default;
            return false;
        }

        public void RegisterBody(EcsPackedEntity entity, Vector3 position, float radius)
        {
            var body = new CollidedBody
            {
                Position = position,
                Radius = radius,
                Entity = entity,
            };
            _bodies.Add(body);
            
            var cellCoordinate = ToCellCoordinate(position);
            for (var y = -1; y <= 1; ++y)
            {
                for (var x = -1; x <= 1; ++x)
                {
                    var currentCellCoordinate = new CellCoordinate(cellCoordinate.X + x, cellCoordinate.Y + y); 
                    if (!_grid.TryGetValue(currentCellCoordinate, out var cell))
                    {
                        cell = new Cell();
                        _grid.Add(currentCellCoordinate, cell);
                    }
                    cell.Bodies.Add(body);
                }
            }
        }

        private CellCoordinate ToCellCoordinate(Vector3 position)
        {
            return new CellCoordinate(Mathf.FloorToInt(position.x / _cellSize),
                Mathf.FloorToInt(position.x / _cellSize));
        }
    }
}