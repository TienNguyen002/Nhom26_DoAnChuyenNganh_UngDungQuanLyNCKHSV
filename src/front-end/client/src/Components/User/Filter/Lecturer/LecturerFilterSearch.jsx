import { useState, useEffect } from "react";
import { Form } from "react-bootstrap";
import { Button } from "bootstrap";
import { Link } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";
import {
  reset,
  updateKeyword,
} from "../../../../Redux/Lecturer";

const LecturerFilterSearch = () => {
  const lecturerFilter = useSelector(state => state.lecturerFilter),
      dispatch = useDispatch();

  const handleReset = (e) => {
    dispatch(reset());
  };

  return (
    <Form
      method="get"
      onReset={handleReset}
      className="row gy-2 gx-3 align-items-center p-2"
    >
      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">Keyword</Form.Label>
        <Form.Control
          type="text"
          placeholder="Nhập từ khóa..."
          name="keyword"
          value={lecturerFilter.keyword}
          onChange={(e) => dispatch(updateKeyword(e.target.value))}
        />
      </Form.Group>
    </Form>
  );
};

export default LecturerFilterSearch;
