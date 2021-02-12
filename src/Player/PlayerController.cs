using Godot;

namespace EvilFarmingGame.Player
{
	public class PlayerController
	{
		private global::Player Body;

		[Export()] public float Speed = 75;
		[Export()] public float SprintSpeed = 112;

		[Export()] public float DraggingSpeed = 50f;
		[Export()] public float SlowerDraggingSpeed = 25f;

		public PlayerController(global::Player Body)
		{
			this.Body = Body;
		}

		public Vector2 InputMovement(float delta) // The Movement method based on Input 
		{
			Vector2 velocity = new Vector2();

			if (Input.IsActionPressed("Player_Up"))
			{
				velocity += Vector2.Up;
				Body.RayPivot.RotationDegrees = 180;
			}
			else if (Input.IsActionPressed("Player_Down"))
			{
				velocity += Vector2.Down;
				Body.RayPivot.RotationDegrees = 0;
			}
			
			if (Input.IsActionPressed("Player_Left"))
			{
				velocity += Vector2.Left;
				Body.RayPivot.RotationDegrees = 90;
			}
			else if (Input.IsActionPressed("Player_Right"))
			{
				velocity += Vector2.Right;
				Body.RayPivot.RotationDegrees = 270;
			}

			if (velocity.Length() > 1)
			{
				velocity.Normalized();
			}

			if (Input.IsActionPressed("Player_Sprint") && Body.Stamina > 0 && !Body.IsDragging)
			{
				velocity *= SprintSpeed;
			}
			else if (Body.IsDragging)
			{
				if (Body.Stamina > 0)
				{
					velocity *= DraggingSpeed;
					Body.Stamina -= 5f * delta;
				}
				else velocity *= SlowerDraggingSpeed;
			}
			else
			{
				velocity *= Speed;
			}

			return Body.MoveAndSlide(velocity);
		}

	}
}
