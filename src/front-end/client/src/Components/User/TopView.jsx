import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./style/user.scss";
import { getTopTopic } from "../../Services/TopicService"
import ShowMoreText from "../Shared/ShowMoreText";

const TopView = () => {
    const [topic, setTopic] = useState([]);

    useEffect(() => {
        getTopTopic().then((data) => {
            if (data) {
                setTopic(data);
            }
            else setTopic([]);
        })
    }, []);

    return (
        <>
            <div className="card">
                <h5 className="text-success text-center">Các đề tài xem nhiều nhất</h5>
                <div className="card-body">
                    {topic.map((item, index) => (
                        <div className="card-topic-content mt-1" key={index}>
                            <Link className="text-decoration-none" to={`/de-tai/${item.urlSlug}`}>
                                <h5>{item.title}</h5>
                            </Link>
                            <div className="card-desc"><ShowMoreText text={item.description} maxLength={50}/></div>
                            <div className="card-department row">
                                <div className="card-department-name col">
                                    Khoa:
                                    <Link className="text-decoration-none px-2" to={`/khoa/${item.department?.urlSlug}`}>
                                        {item.department?.name}
                                    </Link>
                                </div>
                            </div>
                            <div className="col">
                                Lượt xem: {item.viewCount}
                            </div>
                        </div>
                    )
                    )}
                </div>
            </div>

        </>
    )
}

export default TopView;