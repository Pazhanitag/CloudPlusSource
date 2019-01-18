/* eslint no-nested-ternary: 0 */
import styled from 'vue-styled-components';

const wrapperProps = { notificationVisible: Boolean, windowHeight: Number, menuVisible: Boolean };

// (header-notification line-height) + 2x(header-notification padding) + (header height)
const offsetWithNotification = 7.4;
// header height
const offsetWithoutNotification = 5;

const ScrollableContentWrapper = styled('div', wrapperProps)`
  height: ${props => (props.menuVisible ? (props.notificationVisible ? (props.windowHeight - offsetWithNotification) : (props.windowHeight - offsetWithoutNotification)) : props.windowHeight)}rem;
  top: ${props => (props.menuVisible ? (props.notificationVisible ? offsetWithNotification : offsetWithoutNotification) : 0)}rem;
  width: 100%;
  position: absolute;
  background-color: #f2f7f8;
  z-index: 96;
`;

export default ScrollableContentWrapper;
