import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { getTopicsByDepartmentSlug } from "../../../Services/TopicService";
import { Link, useParams } from "react-router-dom";
import format from "date-fns/format";
import { Button } from "react-bootstrap";
import StudentList from "../../../Components/Shared/StudentList";
import ShowMoreText from "../../../Components/Shared/ShowMoreText";

const TopicByDepartment = () => {
  const [topicsList, setTopicsList] = useState([]);
  const params = useParams();
  const { slug } = params;

  let p = 1,
    ps = 5;
  useEffect(
    (ps, p) => {
      getTopicsByDepartmentSlug(slug).then((data) => {
        if (data) {
          setTopicsList(data.items);
        } else setTopicsList([]);
      });
    },
    [ps, p]
  );

  return (
    <>
      <h1>Danh sách đề tài của khoa</h1>
      <Table striped responsive bordered>
        <thead className="table text-center">
          <tr className="table-title">
            <th className="w-25">Tên đề tài</th>
            <th width="200px">Mô tả</th>
            <th>Ngày thực hiện</th>
            <th>Ngày nghiệm thu</th>
            <th>Số người thực hiện</th>
            <th>Khoa</th>
            <th>Giảng viên</th>
            <th>Sinh viên thực hiện</th>
            <th>Trạng thái</th>
          </tr>
        </thead>
        <tbody className="table-content">
          {topicsList.length > 0 ? (
            topicsList.map((item, index) => (
              <tr key={index}>
                <td>
                  <Link
                    className="table-content"
                    to={`/de-tai/${item.urlSlug}`}
                  >
                    {item.title}
                  </Link>
                </td>
                <td>
                  <p className="shortDescription">
                    <ShowMoreText text={item.description} maxLength={50}/>
                  </p>
                </td>
                <td>{format(new Date(item.registrationDate), "dd/MM/yyyy")}</td>
                <td>{format(new Date(item.endDate), "dd/MM/yyyy")}</td>
                <td>{item.studentNumbers}</td>
                <td>{item.department.name}</td>
                <td>{item.lecturer?.fullName}</td>
                <td className="table-content">
                  <StudentList studentList={item.students} />
                </td>
                <td>{item.status.name}</td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={9}>
                <h4>Không tìm thấy đề tài nào</h4>
              </td>
            </tr>
          )}
        </tbody>
      </Table>
    </>
  );
};

export default TopicByDepartment;
