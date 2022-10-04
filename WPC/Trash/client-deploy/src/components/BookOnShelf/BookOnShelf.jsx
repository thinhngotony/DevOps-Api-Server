import React from "react";
import styles from "./BookOnShelf.module.css";

const BookOnShelf = (props) => (
  <div className="col">
    <div className="row" >
      <div className="col" style={{paddingTop: `7.7vh`}}>
        <div className="d-flex justify-content-center align-items-center" style={{fontSize: `1.2vh`}}>
            <div className={styles.circle} style={{fontSize: `0.8vh`}}>{props.rank}</div>
            <div>{props.view_time_in_seconds}/</div>
            <div>{props.view_cnt}</div>
        </div>
        <div className="d-flex justify-content-center" style={{position: 'relative'}}>
          <img className={styles.book} src={props.img || "../parts/noimage.png"}/>
          <div className={styles.noImageText}>{props.img === '' ? props.name : ''}</div>
        </div>
      </div>
    </div>
  </div>
);

BookOnShelf.defaultProps = {};

export default BookOnShelf;
