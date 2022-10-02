import React, {FC} from "react";
import closeIcon from "../../img/close.svg"
import styles from "./Modal.module.css"

interface modalProps{
    onClose: () => void
    title: string
    children: React.ReactNode
}
export const Modal : FC<modalProps> = ({title, children, onClose} : modalProps) => {
    return (
        <div className={styles.modal}>
            <div className={styles.modalOverlay}>
            </div>
            <div className={styles.modalBox}>
                <button className={styles.modalCloseButton} onClick={onClose}>
                    <img src={closeIcon} alt={'close modal'}></img>
                </button>
                <div className={styles.modalTitle}> {title} </div>
                <div className={styles.modalContent}> {children} </div>
            </div>
        <div/>
        </div>
    )
}