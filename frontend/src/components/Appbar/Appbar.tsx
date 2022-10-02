import React, {FC, useState} from "react";
import styles from './Appbar.module.css'


export const Actions: React.FC<appBarProps> = ({onImportClick, onExportClick} : appBarProps) =>{
    return(
        <div className={styles.actionsContainer}>
            <button className={styles.actionButton} onClick={onImportClick}>Добавить таблицу</button>
            <button className={styles.actionButton} onClick={onExportClick}>Экспортировать таблицу</button>
        </div>)
}

interface appBarProps{
    onImportClick: () => void
    onExportClick: () => void
}

export const AppBar : FC<appBarProps> = ({onImportClick, onExportClick} : appBarProps) => {
    return(
        <nav className={styles.appBar}>
            <div className={styles.appName}> PLVisualizer </div>
            <Actions onExportClick={onExportClick} onImportClick={onImportClick}  />
        </nav>
    )
}