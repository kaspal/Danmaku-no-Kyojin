﻿using Danmaku_no_Kyojin.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Danmaku_no_Kyojin.Camera
{
    public class Camera2D
    {
        private const float zoomUpperLimit = 5.0f;
        private const float zoomLowerLimit = .1f;

        private float _zoom;
        private Matrix _transform;
        private Vector2 _pos;
        private float _rotation;
        private int _viewportWidth;
        private int _viewportHeight;
        private int _worldWidth;
        private int _worldHeight;

        public Camera2D(Viewport viewport, int worldWidth,
           int worldHeight, float initialZoom)
        {
            _zoom = initialZoom;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            _viewportWidth = viewport.Width;
            _viewportHeight = viewport.Height;
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
        }

        #region Properties

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < zoomLowerLimit)
                    _zoom = zoomLowerLimit;
                if (_zoom > zoomUpperLimit)
                    _zoom = zoomUpperLimit;
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                float leftBarrier = (float)_viewportWidth *
                       .5f / _zoom;
                float rightBarrier = _worldWidth -
                       (float)_viewportWidth * .5f / _zoom;
                float topBarrier = _worldHeight -
                       (float)_viewportHeight * .5f / _zoom;
                float bottomBarrier = (float)_viewportHeight *
                       .5f / _zoom;
                _pos = value;
                if (_pos.X < leftBarrier)
                    _pos.X = leftBarrier;
                if (_pos.X > rightBarrier)
                    _pos.X = rightBarrier;
                if (_pos.Y > topBarrier)
                    _pos.Y = topBarrier;
                if (_pos.Y < bottomBarrier)
                    _pos.Y = bottomBarrier;
            }
        }

        #endregion

        public void Update()
        {
            // Adjust zoom if the mouse wheel has moved
            if (InputHandler.ScrollUp())
                Zoom += 0.1f;
            else if (InputHandler.ScrollDown())
                Zoom -= 0.1f;

            // Move the camera when the arrow keys are pressed
            Vector2 movement = Vector2.Zero;

            if (InputHandler.KeyDown(Keys.Left))
                movement.X -= 1f;
            if (InputHandler.KeyDown(Keys.Right))
                movement.X += 1f;
            if (InputHandler.KeyDown(Keys.Up))
                movement.Y -= 1f;
            if (InputHandler.KeyDown(Keys.Down))
                movement.Y += 1f;

            this.Pos += movement * 20;
        }

        public Matrix GetTransformation()
        {
            _transform =
               Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                   _viewportHeight * 0.5f, 0));

            return _transform;
        }
    }
}
