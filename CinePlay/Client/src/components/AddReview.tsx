import React, { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Review } from "../Data";
import { addReview } from "../services/ReviewService";

interface AddReviewProps {
  mediaId: string;
}

const CreateReview: React.FC<AddReviewProps> = ({ mediaId }) => {
  const [rating, setRating] = useState<number>(10);
  const [comment, setComment] = useState<string>("");
  const navigate = useNavigate();

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const newReview: Partial<Review> = { mediaId, rating, comment };
    try {
      const addedReview = await addReview(newReview);
      console.log("Review added successfully:", addedReview);
      navigate(`/media/${mediaId}`);
    } catch (error) {
      console.error("Error occurred while adding review:", error);
    }
  };

  return (
    <div className="p-5">
      <h2>Add Review</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="rating" className="form-label">
            Rating:
          </label>
          <input
            type="number"
            className="form-control"
            id="rating"
            value={rating}
            onChange={(e) => setRating(parseInt(e.target.value))}
            min="1"
            max="10"
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="comment" className="form-label">
            Comment:
          </label>
          <textarea
            className="form-control"
            id="comment"
            value={comment}
            onChange={(e) => setComment(e.target.value)}
            rows={4}
          ></textarea>
        </div>
        <button type="submit" className="btn btn-success">
          Submit Review
        </button>
      </form>
    </div>
  );
};

const AddReview: React.FC = () => {
  let { id } = useParams();

  return <CreateReview mediaId={id!} />;
};

export default AddReview;
