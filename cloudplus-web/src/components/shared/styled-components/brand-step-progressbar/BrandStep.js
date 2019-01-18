import styled from 'vue-styled-components';

const brandProps = { color: String, numOfSteps: Number };

// Create a <BrandStep> Vue component that renders a <li> which is
// used as one step in the cloud plus step progress bar
// and is of the passed in color
const BrandStep = styled('li', brandProps)`
  list-style-type: none;
  float: left;
  width: ${props => 100 / props.numOfSteps}%;
  position: relative;
  text-align: center;
  font-size: 15px;

  &::before {
    content: '';
    width: 20px;
    height: 20px;
    line-height: 20px;
    border: 0.0625rem solid ${props => props.color};
    display: block;
    text-align: center;
    margin: 0 auto 10px auto;
    border-radius: 50%;
    background-color: ${props => props.color};
    background-clip: content-box;
    padding: 3px;
  }
  
  &::after {
    content: '';
    position: absolute;
    width: 100%;
    height: 1px;
    background-color: ${props => props.color};
    top: 10px;
    left: -50%; 
    background-clip: content-box; 
    padding:0px 10px 0px 10px;
  }

  &:first-child::after {
    content: none;
  }
`;

export default BrandStep;
