using Godot;

namespace EvilFarmingGame.Player
{
	public class PlayerController
	{
		private global::Player Body; // Accessing the players body

		// The players movement stats
		[Export()] public float Speed = 75;
		[Export()] public float SprintSpeed = 112;

		[Export()] public float DraggingSpeed = 50f;
		[Export()] public float SlowerDraggingSpeed = 25f;

		public enum Direction // For determining the direction the player is facing
		{
			Up=0, Down, Left, Right
		}

		public Direction CurrentDirection; // THe players current facing direction

		public PlayerController(global::Player Body)
		{
			this.Body = Body;
		}
	
		// The Movement method based on Input 
		public Vector2 InputMovement(float delta)
		{
			Vector2 velocity = new Vector2();
			
			// Determine the players movement direction and the facing direction
			if (Input.IsActionPressed("Player_Up"))
			{
				velocity += Vector2.Up;
				CurrentDirection = Direction.Up;
			}
			else if (Input.IsActionPressed("Player_Down"))
			{
				velocity += Vector2.Down;
				CurrentDirection = Direction.Down;
			}

			if (Input.IsActionPressed("Player_Left"))
			{
				velocity += Vector2.Left;
				CurrentDirection = Direction.Left;
			}
			else if (Input.IsActionPressed("Player_Right"))
			{
				velocity += Vector2.Right;
				CurrentDirection = Direction.Right;
			}
			
			if (velocity.Length() > 1) velocity = velocity.Normalized(); // Normalize the players velocity
			
			// Determine movement speed
			if (Body.IsDragging) // Drag the body
			{
				if (Body.Stamina > 0) // Apply a faster dragging speed if the player has enough stamina
				{
					velocity *= DraggingSpeed;
					Body.Stamina -= 5f * delta; // Decrease the players stamina 
				}
				else velocity *= SlowerDraggingSpeed; // Apply a slower dragging speed when the stamina is 0
				
				// Determine the direction based on the dragged bodies angle relative to the player
				var angle = Mathf.Rad2Deg(Body.Position.AngleToPoint(Body.DragginBody.Position)); // Determine the bodies angle relative to the player
				if (angle > -45 && angle < 45) CurrentDirection = Direction.Left; // Direction left
				else if (angle > 45 && angle < 135) CurrentDirection = Direction.Up; // Direction up
				else if (angle > -135 && angle < 135) CurrentDirection = Direction.Down; // Direction Down
				else CurrentDirection = Direction.Right; // Direction right
			}
			else if (Input.IsActionPressed("Player_Sprint") && Body.Stamina > 0) // Apply sprint speed is the player is sprinting and the player has enough stamina 
				velocity *= SprintSpeed;
			else
				velocity *= Speed; // Apply regular waling speed
			
			// Rotate body
			Body.RayPivot.RotationDegrees = CurrentDirection switch
			{
				Direction.Up => 180,
				Direction.Down => 0,
				Direction.Left => 90,
				Direction.Right => 270,
				_=> 0
			};
			
			return Body.MoveAndSlide(velocity); // Apply the players velocity velocity and return the result
		}

	}
}
