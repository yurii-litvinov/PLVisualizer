import styled from 'styled-components'
import { FC, useState } from "react";
import { Draggable } from "react-beautiful-dnd";
import { Discipline } from "../../Models/Discipline";
import { Card, CardHeader, CardContent, Typography, Table, TableHead, TableRow, TableBody, TableCell } from '@mui/material';
import Collapse from '@mui/material/Collapse';
import IconButton, { IconButtonProps } from '@mui/material/IconButton';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

interface ExpandMoreProps extends IconButtonProps {
    expand: boolean;
}

const ExpandMore = styled((props: ExpandMoreProps) => {
    const { expand, ...other } = props;
    return <IconButton {...other} />;
})(({ theme, expand }) => ({
    transform: !expand ? 'rotate(0deg)' : 'rotate(180deg)',
    marginLeft: 'auto',
}));

interface DisciplineProps {
    discipline: Discipline
    index: number
}

interface ContainerProps {
    readonly isDragging: boolean
}

export const DndDiscipline : FC<DisciplineProps> =  ({discipline, index} : DisciplineProps) => {
    const [expanded, setExpanded] = useState(false);

    const handleExpandClick = () => {
        setExpanded(!expanded);
      };

    return (
        <Draggable draggableId={discipline.id.toString()} index={index}>
            { (provided, snapshot) =>
                <Container
                    {...provided.draggableProps}
                    {...provided.dragHandleProps}
                    ref={provided.innerRef}
                    isDragging={snapshot.isDragging}>
                    <Card>
                        <CardHeader
                            action={
                                <ExpandMore 
                                    expand={expanded}
                                    onClick={handleExpandClick}
                                    aria-expanded={expanded}
                                    aria-label="show more"
                                    >
                                    <ExpandMoreIcon />
                                </ExpandMore>
                            }
                            title={`[${discipline.code}] ${discipline.name}`}
                            titleTypographyProps={{ fontSize: 15}}
                            subheader={`${discipline.generalWorkType.toLowerCase()}, ${discipline.term.toLowerCase()}, ${discipline.totalLoad} часов`}
                            subheaderTypographyProps={{ fontSize: 13, color: "text.secondary"}}
                        />
                        <CardContent sx={{ mt: -3 }}>
                            <Typography sx={{ fontSize: 13 }} color="text.secondary" gutterBottom>
                                Контингент: {discipline.audience}
                            </Typography>
                            <Collapse in={expanded}>
                                <Table size="small">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell style={{width: "40%"}}>Нагрузка</TableCell>
                                            <TableCell style={{width: "10"}}>Часы</TableCell>
                                            <TableCell style={{width: "50"}}>Контингент</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                            { discipline.loadDetails.map(item =>
                                                    <TableRow>
                                                        <TableCell>
                                                            <Typography sx={{ fontSize: 13 }} color="text.secondary" gutterBottom>
                                                                {item.loadType}
                                                            </Typography>
                                                        </TableCell>
                                                        <TableCell>
                                                            <Typography sx={{ fontSize: 13 }} color="text.secondary" gutterBottom>
                                                                {item.hours}
                                                            </Typography>
                                                        </TableCell>
                                                        <TableCell>
                                                            <Typography sx={{ fontSize: 13 }} color="text.secondary" gutterBottom>
                                                                {item.audience}
                                                            </Typography>
                                                        </TableCell>
                                                    </TableRow>
                                                )
                                            }
                                    </TableBody>
                                </Table>
                            </Collapse>
                        </CardContent>
                    </Card>
                </Container>}
        </Draggable>
    )
}

const Container = styled.div<ContainerProps>`
  font-size: 11px;
    padding: 8px;
    margin-bottom: 8px;
    border-radius: 2px;
    background-color: ${props => (props.isDragging ? 'lightgreen' : 'white')}
`