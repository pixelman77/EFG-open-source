using Godot;

namespace EvilFarmingGame.Player
{
	public class PlayerController
	{
		private global::Player Body;

		[Export] public float Speed = 75;
		[Export] public float SprintSpeed = 112;
		
		public PlayerController(global::Player Body)
		{
			this.Body = Body;
		}

		public Vector2 InputMovement(float delta) // The Movement method based on Input 
		{
			Vector2 Velocity = new Vector2();

			if (Input.IsActionPressed("Player_Up"))
			{
				Velocity += Vector2.Up;
				Body.RayPivot.RotationDegrees = 180;
			}
			else if (Input.IsActionPressed("Player_Down"))
			{
				Velocity += Vector2.Down;
				Body.RayPivot.RotationDegrees = 0;
			}
			if (Input.IsActionPressed("Player_Left"))
			{
				Velocity += Vector2.Left;
				Body.RayPivot.RotationDegrees = 90;
			}
			else if (Input.IsActionPressed("Player_Right"))
			{
				Velocity += Vector2.Right;
				Body.RayPivot.RotationDegrees = 270;
			}
			if (Velocity.Length() > 1)
			{
				Velocity.Normalized();
			}

			if (Input.IsActionPressed("Player_Sprint"))
				Velocity *= SprintSpeed;
			else
				Velocity *= Speed;
			
			return Body.MoveAndSlide(Velocity);
		}
		
	}
}
